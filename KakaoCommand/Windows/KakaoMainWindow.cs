using KakaoCommand.Common;
using KakaoCommand.Interop;
using KakaoCommand.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KakaoCommand.Windows
{
    class KakaoMainWindow : Window
    {
        Window _tabWindow;
        KakaoListWindow _contactWindow;
        KakaoListWindow _chatWindow;
        KakaoTabWindow _moreWindow;

        internal KakaoMainWindow(IntPtr hwnd) : base(hwnd)
        {
            IntPtr tabHwnd = UnsafeNativeMethods.GetWindow(hwnd, NativeMethods.GetWindowType.GW_CHILD);
            IntPtr contactHwnd = UnsafeNativeMethods.GetWindow(tabHwnd, NativeMethods.GetWindowType.GW_CHILD);
            IntPtr chatHwnd = UnsafeNativeMethods.GetWindow(contactHwnd, NativeMethods.GetWindowType.GW_HWNDNEXT);
            IntPtr moreHwnd = UnsafeNativeMethods.GetWindow(chatHwnd, NativeMethods.GetWindowType.GW_HWNDNEXT);

            _tabWindow = new Window(tabHwnd);
            _contactWindow = new KakaoListWindow(this, contactHwnd);
            _chatWindow = new KakaoListWindow(this, chatHwnd);
            _moreWindow = new KakaoTabWindow(this, moreHwnd);
        }

        public void SetTab(KakaoTab tab)
        {
            int x = 36 + (int)tab * 58;
            int y = 30;

            _tabWindow.Touch(x, y);
        }

        public void Search(KakaoTab tab, string text)
        {
            if (tab == KakaoTab.More)
                return;

            if (tab == KakaoTab.Contact)
                _contactWindow.Search(text);
            else if (tab == KakaoTab.ChatRoom)
                _chatWindow.Search(text);
        }

        public IEnumerable<KakaoChatWindow> GetChatRooms(string nickname)
        {
            IntPtr handle = Handle;

            while (handle != IntPtr.Zero) 
            {
                handle = UnsafeNativeMethods.GetWindow(handle, NativeMethods.GetWindowType.GW_HWNDNEXT);

                if (handle == IntPtr.Zero)
                    continue;

                if (!KakaoChatWindow.IsChatWindow(handle))
                    continue;
                
                if (!string.IsNullOrEmpty(nickname))
                {
                    string title = GetWindowText(handle);

                    if (!title.Contains(nickname))
                        continue;
                }

                yield return new KakaoChatWindow(handle);
            }
        }

        public void OpenChatRoom(string nickname)
        {
            var chatWindow = GetChatRooms(nickname).FirstOrDefault();

            if (chatWindow == null)
            {
                Search(KakaoTab.Contact, nickname);
                _contactWindow.SearchList.KeyPress(Keys.Enter);
                //Search(KakaoTab.Contact, "");
            }
            else
            {
                chatWindow.Show();
                chatWindow.Activate();
            }
        }
    }
}
