using KakaoCommand.Interop;
using System;
using System.Runtime.InteropServices;

namespace KakaoCommand.Utility
{
    static class ProcessUtility
    {
        public static void StartNoActivate(string fileName)
        {
            var startInfo = new NativeMethods.STARTUPINFO();
            startInfo.cb = Marshal.SizeOf(startInfo);
            startInfo.dwFlags = (int)NativeMethods.STARTF.UseShowWindow;
            startInfo.wShowWindow = (short)NativeMethods.ShowWindowCommands.ShowNoActivate;

            var processInfo = new NativeMethods.PROCESS_INFORMATION();

            var pSec = new NativeMethods.SECURITY_ATTRIBUTES();
            var tSec = new NativeMethods.SECURITY_ATTRIBUTES();

            pSec.nLength = Marshal.SizeOf(pSec);
            tSec.nLength = Marshal.SizeOf(tSec);

            UnsafeNativeMethods.CreateProcess(
                fileName, 
                null, 
                ref pSec,
                ref tSec,
                true, 
                NativeMethods.CreationFlags.NONE, 
                IntPtr.Zero, 
                null, 
                ref startInfo, 
                out processInfo);

            UnsafeNativeMethods.WaitForInputIdle(processInfo.hProcess, int.MaxValue);

            UnsafeNativeMethods.CloseHandle(processInfo.hProcess);
            UnsafeNativeMethods.CloseHandle(processInfo.hThread);
        }
    }
}
