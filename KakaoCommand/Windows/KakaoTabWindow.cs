using KakaoCommand.Utility;
using System;

namespace KakaoCommand.Windows
{
    class KakaoTabWindow : Window
    {
        KakaoMainWindow _parent;

        internal KakaoTabWindow(KakaoMainWindow parent, IntPtr hwnd) : base(hwnd)
        {
            _parent = parent;
        }
    }
}
