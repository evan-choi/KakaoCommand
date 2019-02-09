using KakaoCommand.Interop;
using KakaoCommand.Utility;
using System;

namespace KakaoCommand.Windows
{
    class KakaoChatWindow : Window
    {
        const string chatWindowClass = "#32770";
        const string richEditClassName = "RichEdit20W";

        internal KakaoChatWindow(IntPtr hwnd) : base(hwnd)
        {
        }

        public static bool IsChatWindow(IntPtr hwnd)
        {
            if (GetClassName(hwnd) != chatWindowClass)
                return false;

            IntPtr editHwnd = UnsafeNativeMethods.GetWindow(hwnd, NativeMethods.GetWindowType.GW_CHILD);

            return GetClassName(editHwnd) == richEditClassName;
        }
    }
}
