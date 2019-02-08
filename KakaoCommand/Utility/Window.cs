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
