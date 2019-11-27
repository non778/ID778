//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using Newtonsoft.Json.Linq;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Width of output drawing
        /// </summary> 640
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary> 480
        private const float RenderHeight = 486.0f;

        /// <summary>
        /// Thickness of drawn joint lines 선의 두께
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse 점 두께
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles 인식이 제대로 되지 않았을 경우의 테두리 두께 (빨간색)
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point 골격의 중심점 그리는 브러쉬 객체
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked 현재 추적되는 관절을 그리는데 사용되는 브러쉬 객체(초록)
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred 유추된 관절을 그리는데 사용되는 브러시(노랑)
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked 현재 추적되는 뼈를 그리는데 사용되는 펜(초록)
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred 유추되는 뼈를 그리는데 사용되는 펜(흰)
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Active Kinect sensor 키넥트 센서
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Drawing group for skeleton rendering output 스켈레톤 렌더링 출력을 위한 그룹 드로잉
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display 스켈레톤을 표시할 그림 이미지
        /// </summary>
        private DrawingImage imageSource;

        // Gesture 객체 생성
        private Gesture gesture = new Gesture();

        private static string path = "C:\\Users\\user\\Desktop\\test\\test1.txt";

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute startup tasks 시작 작업
        /// </summary>
        /// <param name="sender">object sending the event</param> 이벤트를 보내는 객체
        /// <param name="e">event arguments</param> 이벤트를 받는
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected) // 사용준비 완벽하게 완료 됬는지
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames 스켈레톤 스트림을 켜서 스켈레톤 프레임을 받음
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data 새로운 컬러 프레임 데이터가 있을때마다 호출될 이벤트 핸들러 추가
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Start the sensor! // 센서 시작
                try
                {
                    this.sensor.Start();
                    this.sensor.DepthStream.Range = DepthRange.Near;
                    this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;                
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

        }

        /// <summary>
        /// Execute shutdown tasks 스켈레톤 종료작업
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event 키넥트 센서의 스켈레톤 프레임 준비 이벤트를 위한 이벤트 핸들러
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {

            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength]; // skeletonFrame의 골격데이터 버퍼의 총 길이로 배열 생성
                    skeletonFrame.CopySkeletonDataTo(skeletons); // 스켈레톤 데이터를 배열로 복사
                }
            }
            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size 랜더링 크기를 설정하기 위해 투명한 배경을 그림(검정)
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));


                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly) // 골격 추적은 되지만 일반적인 위치만 알고 특정 관절 위치를 모를때
                        {
                            dc.DrawEllipse( // DrawingContext의 골격 원을 디자인
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position), // 골격의 위치를 가져오거나 설정
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight)); // 인식한 스켈레톤을 화면에 나타내줄수 있는 범위 
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints 골격의 뼈와 관절을 그리는 메서드
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param> 그리기, 푸시 및 팝 명령을 사용하여 시각적 콘텐츠를 설명합니다.
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            Joint[] joints = {skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.ShoulderRight]}; // joint 스켈레톤의 관절을 설명하는데 사용

            
            foreach(Joint joint in joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness); // 타원을 그림
                }
            }                           
            //ToFile(skeleton.Joints[JointType.HandRight].Position.X, skeleton.Joints[JointType.HandRight].Position.Y);  // 체크------------------------------------------------------------------------

            lbl_chk.Content = gesture.Gesture_Algorithm(skeleton);
            lbl.Content = gesture.getStrdata2();
            lbl_frame.Content = gesture.getFrame();
            lbl_datachk.Content = gesture.getTest();

        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point 렌더링 공간 안에 놓이도록 스켈레톤포인트 매핑 및 포인트 변환
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints 두 관절 사이의 뼈 선을 그림
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param> // 스켈레톤의 모든 관절 유형
        /// <param name="jointType1">joint to end drawing at</param> // 스켈레톤의 모든 관절 유형
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked) // 관절 추적이 아예 안될 시
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred) // 관절 추적은 되나 신뢰도가 낮을때
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked) // 관절 추정이 되고 신로도가 높을 때
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position)); // 점과 점을 잇는 선 그림
        }
        

        public static void ToFile(float x, float y)
        {

            JObject jObject = new JObject();
            //string strdata = null;
            using (StreamWriter Sw = new StreamWriter(new FileStream(path, FileMode.Append), Encoding.UTF8))
            {

                //Sw.Write(jObject.ToString());
            }

        }

        public void GestureEx()
        {

        }

        /*
        public GesturePartResult UP_C(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y >
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X ==
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }
            }

            // Hand dropped
            return GesturePartResult.Failed;
        }

        public GesturePartResult Down_C(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y <
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X ==
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }
            }

            // Hand dropped
            return GesturePartResult.Failed;
        }

        public GesturePartResult Right_C(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y ==
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X >
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }
            }

            // Hand dropped
            return GesturePartResult.Failed;
        }

        public GesturePartResult Left_C(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y ==
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X <
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return GesturePartResult.Succeeded;
                }
            }

            // Hand dropped
            return GesturePartResult.Failed;
        }

        public enum GesturePartResult
        {
            Failed,
            Succeeded
        }
        */
    }
}