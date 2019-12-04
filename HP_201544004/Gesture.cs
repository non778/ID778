using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    class Gesture
    {
        Queue<string> que = new Queue<string>(); // 어깨 기준 구역 (A, B, C, D)
        string strdata = null; // 이전 값
        string strdata2 = " "; // 이전 값과 현재값을 합친 변수
        string predata = null; // 이전 제스쳐
        string nextdata = null; // 현재 제스쳐
        int changeStatechkflag = 0; // "U_RL", "U_LR" 제스쳐 flag
        int frame = 0; // 프레임 단위 타임아웃 체크
        int dataFrame = 0; // 프레임 단위 제스쳐 전달 체크
        string keyWord;
        bool chkClick = false;
        int playchk = 1;

        int test = 0;

        public Gesture()
        {

        }

        public void SetKeyWord(string KeyWord)
        {
            this.keyWord = KeyWord;
        }

        public string GetKeyWord()
        {
            return this.keyWord;
        }

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

        public string getStrdata2()
        {
            return strdata2;
        }

        public string getFrame()
        {
            return frame.ToString();
        }

        public string getTest()
        {
            return test.ToString();
        }

        public string Gesture_Algorithm(Skeleton skeleton)
        {
            //제스쳐 프로토타이핑
            /*
            temp1 = check(skeleton);
            temp2 = temp2.Substring(temp2.Length - 1) + temp1;
            if (temp2 == "가나") lbl_chk.Content = "U_LR";
            if (temp2 == "나가") lbl_chk.Content = "U_RL";
            lbl.Content = temp2;
            */
            
            string gestureData = null;

            if (Check(skeleton) != "")
            {
                this.setQue(Check(skeleton));
            }
            if(que.Count > 0)
            {
                strdata = que.Dequeue();
                strdata2 = strdata2.Substring(strdata2.Length - 1) + strdata;
            }
                

            if (!ChangeofState(strdata2))
            {
                frame++;
            }
            else
            {
                frame = 0;
            }
            if (frame > 50)
            {
                changeStatechkflag = 0;
                test = 0; // 삭제
                return null;
            }
            if (changeStatechkflag > 0)
            {
                if (dataFrame > 7)
                {
                    dataFrame = 0;
                    test++; // 삭제
                    return "U_LR";
                }
                dataFrame++;
                if (strdata2 == "BA")
                {
                    changeStatechkflag = -1;
                    dataFrame = 0;
                    test = 1; // 삭제
                    return "U_RL";
                }
                return "U_LR";
            }
            if (changeStatechkflag < 0)
            {
                if (dataFrame > 7)
                {
                    dataFrame = 0;
                    test++; // 삭제
                    return "U_RL";
                }
                dataFrame++;
                if (strdata2 == "AB")
                {
                    changeStatechkflag = 1;
                    dataFrame = 0; 
                    test = 1; // 삭제
                    return "U_LR";
                }
                return "U_RL";
            }

            switch (strdata2)
            {
                case "AB":
                    changeStatechkflag = 1;
                    gestureData = "U_LR";
                    test++; // 삭제
                    break;
                case "BA":
                    changeStatechkflag = -1;
                    test++; // 삭제
                    gestureData = "U_RL";
                    break;
                case "CD":
                    gestureData = "D_LR";
                    break;
                case "DC":
                    gestureData = "D_RL";
                    break;
                //default: return "xx";
            }

            return gestureData;
        }

        public bool start_stop(Skeleton skeleton)
        {
            if (Check(skeleton) != "")
            {
                this.setQue(Check(skeleton));
            }
            if (que.Count > 0)
            {
                strdata = que.Dequeue();
                strdata2 = strdata2.Substring(strdata2.Length - 1) + strdata;
            }

            if (strdata2 == "DB")
            {
                playchk = playchk * -1;
            }

            if(playchk < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ChangeofState(string strdata)
        {
            predata = nextdata;
            nextdata = strdata;

            if(predata == nextdata)
            {
                return false;
            }
            return true;
        }

        public string Check(Skeleton skeleton)
        {
            string strdata = null;

            strdata = UP_L(skeleton);
            strdata += UP_R(skeleton);
            strdata += Down_L(skeleton);
            strdata += Down_R(skeleton);
            return strdata.Trim();
        }

        public string UP_R(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.WristRight].Position.Y >
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand right of shoulder
                if (skeleton.Joints[JointType.WristRight].Position.X >
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
            if (skeleton.Joints[JointType.WristRight].Position.Y >
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.WristRight].Position.X <
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "A";
                }
            }

            // Hand dropped
            return " ";
        }

        // 제스쳐 메소드 프로토타이핑
        /*
        public string check(Skeleton skeleton)
        {
            string strdata;

            strdata = UP_R(skeleton);
            strdata += UP_L(skeleton);
            return strdata.Trim();
        }
        */
        public string Down_R(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.WristRight].Position.Y <
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.WristRight].Position.X >
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "D";
                }
            }

            // Hand dropped
            return " ";
        }

        public string Down_L(Skeleton skeleton)
        {
            // Hand above shoulder
            if (skeleton.Joints[JointType.WristRight].Position.Y <
                skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                // Hand left of shoulder
                if (skeleton.Joints[JointType.WristRight].Position.X <
                    skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    return "C";
                }
            }

            // Hand dropped
            return " ";
        }

        int leftchk = 0;

        // 마우스 이벤트
        public bool Mouse_Click(Skeleton skeleton)
        {
            if(skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.ShoulderRight].Position.Y)
            {
                if(skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ShoulderRight].Position.X)
                {
                    leftchk = 1;
                }
                else
                {
                    leftchk = 0;
                }
            }
            else
            {
                leftchk = 0;
            }

            if (/*skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.WristRight].Position.Y &&*/ leftchk == 1)
            {
                return chkClick = true;
            }
            else
            {
                return false;
            }     
        }

        public bool UNMouse_Click(Skeleton skeleton)
        {
            if (/*skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.WristRight].Position.Y ||*/ leftchk == 0)
            {
                return chkClick = true;
            }
            else
            {
                return false;
            }
        }
    }
}
