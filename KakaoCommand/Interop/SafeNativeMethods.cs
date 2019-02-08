using System;
using System.Runtime.InteropServices;

namespace KakaoCommand.Interop
{
    static class SafeNativeMethods
    {
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    }
}
