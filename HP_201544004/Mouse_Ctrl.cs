using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Runtime.InteropServices;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{

    static class Mouse_Ctrl
    {
        private const uint LBDOWN = 0x00000002; // 왼쪽 마우스 버튼 눌림
        private const uint LBUP = 0x00000004; // 왼쪽 마우스 버튼 떼어짐

        [DllImport("user32.dll")] // 입력 제어
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")] // 커서 위치 제어
        static extern int SetCursorPos(int x, int y);
    }
}
