using KakaoCommand.Common;
using KakaoCommand.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KakaoCommand.Utility
{
    public class Window
    {
        public IntPtr Handle { get; }

        public bool Visible => UnsafeNativeMethods.IsWindowVisible(Handle);

        public string Title => GetWindowText(Handle);

        public string Class => GetClassName(Handle);

        public Size Size
        {
            get
            {
                var info = GetWindowInfo(Handle);
                return new Size(info.rcClient.Width, info.rcClient.Height);
            }
        }

        public Point Location
        {
            get
            {
                var info = GetWindowInfo(Handle);
                return new Point(info.rcClient.Left, info.rcClient.Top);
            }
        }

        internal Window(IntPtr hwnd)
        {
            Handle = hwnd;
        }

        public Bitmap Capture()
        {
            Size size = Size;
            var rect = new Rectangle();

            rect.Width = size.Width;
            rect.Height = size.Height;

            return Screen.Capture(Handle, rect);
        }

        public Point PositionToClient(Point position)
        {
            position.X -= Location.X;
            position.Y -= Location.Y;
            
            return position;
        }

        public void TouchDown(int x, int y)
        {
            int tx = x;
            int ty = y;

            TouchOffset(ref tx, ref ty);

            UnsafeNativeMethods.SendMessage(
                Handle,
                NativeMethods.WM.LBUTTONDOWN,
                0,
                MakeLParam(tx, ty));

            TouchMove(x, y);
        }

        public void TouchUp(int x, int y)
        {
            TouchOffset(ref x, ref y);

            UnsafeNativeMethods.SendMessage(
                Handle,
                NativeMethods.WM.LBUTTONUP,
                0,
                MakeLParam(x, y));
        }

        public void TouchMove(int x, int y)
        {
            TouchOffset(ref x, ref y);

            UnsafeNativeMethods.SendMessage(
                Handle,
                NativeMethods.WM.MOUSEMOVE,
                NativeMethods.MK_LBUTTON,
                MakeLParam(x, y));
        }

        public void Touch(int x, int y)
        {
            lock (this)
            {
                TouchDown(x, y);
                TouchUp(x, y);
            }
        }

        public void KeyDown(Keys key)
        {
            UnsafeNativeMethods.PostMessage(
                GetInputHandle(),
                NativeMethods.WM.KEYDOWN,
                (int)key,
                0);
        }

        public void KeyUp(Keys key)
        {
            UnsafeNativeMethods.PostMessage(
                GetInputHandle(),
                NativeMethods.WM.KEYUP,
                (int)key,
                0);
        }

        public void KeyPress(Keys key)
        {
            KeyDown(key);
            KeyUp(key);
        }

        protected virtual void TouchOffset(ref int x, ref int y)
        {
        }

        protected virtual IntPtr GetInputHandle()
        {
            return Handle;
        }

        int MakeLParam(int low, int high)
        {
            return (high << 16) | (low & 0xFFFF);
        }

        public void Show()
        {
            UnsafeNativeMethods.PostMessage(Handle, NativeMethods.WM.SYSCOMMAND, (int)NativeMethods.SysCommands.SC_RESTORE, 0);
        }

        public void Activate()
        {
            UnsafeNativeMethods.SetForegroundWindow(Handle);
        }

        public override string ToString()
        {
            return $"{Title} ({Class})";
        }

        internal static string GetWindowText(IntPtr hWnd)
        {
            var buffer = new StringBuilder(128);
            UnsafeNativeMethods.GetWindowText(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
        }

        internal static string GetClassName(IntPtr hWnd)
        {
            var buffer = new StringBuilder(128);
            UnsafeNativeMethods.GetClassName(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
        }

        internal static NativeMethods.WINDOWINFO GetWindowInfo(IntPtr hWnd)
        {
            var info = new NativeMethods.WINDOWINFO();

            UnsafeNativeMethods.GetWindowInfo(hWnd, ref info);

            return info;
        }

        public static IEnumerable<Window> GetChildWindows(IntPtr hwndParent)
        {
            var result = new List<IntPtr>();
            var resultHandle = GCHandle.Alloc(result);
            
            try
            {
                UnsafeNativeMethods.EnumChildWindows(hwndParent, EnumChildWindow, GCHandle.ToIntPtr(resultHandle));
            }
            finally
            {
                if (resultHandle.IsAllocated)
                    resultHandle.Free();
            }

            return result.Select(hWnd => new Window(hWnd));
        }

        static bool EnumChildWindow(IntPtr hwnd, IntPtr lParam)
        {
            var resultHandle = GCHandle.FromIntPtr(lParam);

            if (resultHandle.Target is List<IntPtr> result)
                result.Add(hwnd);

            return true;
        }
    }
}
