using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

//using KinectMouseController;

namespace 自製精靈XD
{
    public class My_Mouse
    {
        #region 設定滑鼠位置
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        //用法
        //SetCursorPos(1024, 768);
        #endregion
        #region 模擬滑鼠事件
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;      //左按下
        private const int MOUSEEVENTF_LEFTUP = 0x04;        //左抬起
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;   //右按下
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;     //右抬起
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  //中按下
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;    //中抬起
        #endregion


        static public void moveto(int x, int y)
        {
            moveto_code(x, y, 0);
        }

        /// <summary>
        /// 滑鼠移動的code
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="count">幾步未到達則中斷</param>
        static void moveto_code(int x, int y, int count)
        {
            if (count > 200)
                return;

            int movesizeX = 15;
            int movesizeY = 15;
            System.Threading.Thread.Sleep(3);

            if (Math.Abs(Cursor.Position.X - x) < movesizeX)
                movesizeX = 1;
            if (Math.Abs(Cursor.Position.Y - y) < movesizeY)
                movesizeY = 1;


            if (Cursor.Position.X > x)
                Cursor.Position = new Point(Cursor.Position.X - movesizeX, Cursor.Position.Y);
            else if (Cursor.Position.X < x)
                Cursor.Position = new Point(Cursor.Position.X + movesizeX, Cursor.Position.Y);
            if (Cursor.Position.Y > y)
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - movesizeY);
            else if (Cursor.Position.Y < y)
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + movesizeY);
            //MessageBox.Show(Cursor.Position.X.ToString() + " Y: " + Cursor.Position.Y.ToString());
            if (x != Cursor.Position.X || y != Cursor.Position.Y)
                moveto_code(x, y, count + 1);
        }




        #region 單按系列

        static public void LeftClick(int x, int y)
        {
            LeftDown(x, y);
            System.Threading.Thread.Sleep(20);
            LeftUp(x, y);
        }

        static public void RightClick(int x, int y)
        {
            RightDown(x, y);
            System.Threading.Thread.Sleep(20);
            RightUp(x, y);
        }

        static public void MiddleClick(int x, int y)
        {
            MiddleDown(x, y);
            System.Threading.Thread.Sleep(20);
            MiddleUp(x, y);
        }

        #endregion

        #region 壓住系列

        static public void LeftDown(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
        }

        static public void RightDown(int x, int y)
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
        }

        static public void MiddleDown(int x, int y)
        {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0);
        }

        #endregion

        #region 放開系列

        static public void LeftUp(int x, int y)
        {
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        static public void RightUp(int x, int y)
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
        }

        static public void MiddleUp(int x, int y)
        {
            mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0);
        }

        #endregion




        #region KinectMouseController元件的使用方法
        /*
        // KinectMouseController元件的使用方法
        static public void LeftClick2(int x, int y)
        {
            //int x = Cursor.Position.X;
            //int y = Cursor.Position.Y;
            KinectMouseController.KinectMouseMethods.SendMouseInput(x, y, (int)SystemInformation.PrimaryMonitorSize.Width, (int)SystemInformation.PrimaryMonitorSize.Height, true);
            Thread.Sleep(1000);
            KinectMouseController.KinectMouseMethods.SendMouseInput(x, y, (int)SystemInformation.PrimaryMonitorSize.Width, (int)SystemInformation.PrimaryMonitorSize.Height, false);
        }
        */
        #endregion

    }

}
