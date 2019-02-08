using KakaoCommand.Interop;
using System;
using System.Drawing;

namespace KakaoCommand.Utility
{
    static class Screen
    {
        public static Bitmap Capture(IntPtr target, Rectangle bound)
        {
            IntPtr hBitmap;
            IntPtr hDC = UnsafeNativeMethods.GetDC(target);
            IntPtr hMemDC = UnsafeNativeMethods.CreateCompatibleDC(hDC);

            var info = new NativeMethods.WINDOWINFO();
            UnsafeNativeMethods.GetWindowInfo(target, ref info);
            
            hBitmap = UnsafeNativeMethods.CreateCompatibleBitmap(hDC, bound.Width, bound.Height);

            if (hBitmap != IntPtr.Zero)
            {
                IntPtr hObj = UnsafeNativeMethods.SelectObject(hMemDC, hBitmap);

                UnsafeNativeMethods.BitBlt(
                    hMemDC, 0, 0, bound.Width, bound.Height, 
                    hDC, bound.Left, bound.Top, 
                    NativeMethods.SRCCOPY);

                UnsafeNativeMethods.SelectObject(hMemDC, hObj);
                UnsafeNativeMethods.DeleteDC(hMemDC);
                UnsafeNativeMethods.ReleaseDC(UnsafeNativeMethods.GetDesktopWindow(), hDC);

                var result = Image.FromHbitmap(hBitmap);

                UnsafeNativeMethods.DeleteObject(hBitmap);

                return result;
            }

            return null;
        }
    }
}
