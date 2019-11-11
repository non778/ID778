using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    class Gesture
    {
        Queue<string> que = new Queue<string>();

        public void setQue(string strdata)
        {
            if(this.que.Count != 2)
            {
                this.que.Enqueue(strdata);
            }
            else
            {
                this.que.Enqueue(strdata);
                this.que.Dequeue();
            }
           
        }

        public Queue<String> getQue()
        {
            return que;
        }

        public void algorithm(Skeleton skeleton)
        {
            //제스쳐 프로토타이핑
            /*
            temp1 = check(skeleton);
            temp2 = temp2.Substring(temp2.Length - 1) + temp1;
            if (temp2 == "가나") lbl_chk.Content = "U_LR";
            if (temp2 == "나가") lbl_chk.Content = "U_RL";
            lbl.Content = temp2;
            */

            string temp1 = null;
            string temp2 = null;


        }

        public string UP_R(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y >
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand right of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X >
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "B";
                }
            }

            // Hand dropped
            return " ";
        }

        public string UP_L(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y >
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X <
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "A";
                }
            }

            // Hand dropped
            return " ";
        }

        // 제스쳐 메소드 프로토타이핑
        public string check(Skeleton skeleton)
        {
            string strdata;

            strdata = UP_R(skeleton);
            strdata += UP_L(skeleton);
            return strdata.Trim();
        }
        public string Down_R(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y <
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X >
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "C";
                }
            }

            // Hand dropped
            return " ";
        }

        public string Down_L(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Y <
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.HandRight].Position.X <
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "D";
                }
            }

            // Hand dropped
            return " ";
        }
    }
}
