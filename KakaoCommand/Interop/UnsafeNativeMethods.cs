using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace KakaoCommand.Interop
{
    static class UnsafeNativeMethods
    {
        [DllImport(ExternDll.User32, ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SendInput(uint nInputs, NativeMethods.Input[] pInputs, int cbSize);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(ExternDll.User32, ExactSpelling = true)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, NativeMethods.EnumChildrenCallback lpEnumFunc, IntPtr lParam);

        [DllImport(ExternDll.User32)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport(ExternDll.User32, SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref NativeMethods.WINDOWINFO pwi);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, NativeMethods.WM msg, int wParam, int lParam);

        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, NativeMethods.WM msg, int wParam, int lParam);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport(ExternDll.User32)]
        public static extern int GetSystemMetrics(NativeMethods.SM nIndex);

        [DllImport(ExternDll.User32)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int width, int height);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr bmp);

        [DllImport(ExternDll.Gdi32)]
        public static extern bool BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        [DllImport(ExternDll.Gdi32)]
        public static extern IntPtr DeleteObject(IntPtr hDc);
    }
}