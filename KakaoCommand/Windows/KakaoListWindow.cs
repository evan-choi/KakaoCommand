using KakaoCommand.Interop;
using KakaoCommand.Utility;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace KakaoCommand.Windows
{
    class KakaoListWindow : KakaoTabWindow
    {
        public Window Edit { get; }

        public Window List { get; }

        public Window SearchList { get; }

        internal KakaoListWindow(KakaoMainWindow parent, IntPtr hwnd) : base(parent, hwnd)
        {
            IntPtr editHwnd = UnsafeNativeMethods.GetWindow(hwnd, NativeMethods.GetWindowType.GW_CHILD);
            IntPtr searchListHwnd = UnsafeNativeMethods.GetWindow(editHwnd, NativeMethods.GetWindowType.GW_HWNDNEXT);
            IntPtr listHwnd = UnsafeNativeMethods.GetWindow(searchListHwnd, NativeMethods.GetWindowType.GW_HWNDNEXT);

            Edit = new Window(editHwnd);
            SearchList = new Window(searchListHwnd);
            List = new Window(listHwnd);
        }

        public string GetSearch()
        {
            int length = UnsafeNativeMethods.SendMessage(Edit.Handle, NativeMethods.WM.GETTEXTLENGTH, 0, 0).ToInt32();

            if (length == 0)
                return string.Empty;

            var text = new StringBuilder(length + 1);

            UnsafeNativeMethods.SendMessage(Edit.Handle, NativeMethods.WM.GETTEXT, text.Capacity, text);

            return text.ToString();
        }

        public void Search(string text)
        {
            bool oldEmpty = string.IsNullOrEmpty(GetSearch());
            bool newEmpty = string.IsNullOrEmpty(text);

            IntPtr textHandle = Marshal.StringToHGlobalUni(text);
            UnsafeNativeMethods.SendMessage(Edit.Handle, NativeMethods.WM.SETTEXT, IntPtr.Zero, textHandle);

            if (oldEmpty != newEmpty)
            {
                if (Visible)
                {
                    while (List.Visible != newEmpty)
                        Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
