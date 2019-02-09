using KakaoCommand.Common;
using KakaoCommand.Interop;
using KakaoCommand.Utility;
using KakaoCommand.Windows;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KakaoCommand
{
    static class Kakao
    {
        const string mainWindowClassName = "EVA_Window_Dblclk";

        static Process _process;

        public static void Open()
        {
            if (!CheckPathValid())
                return;

            var p = GetProcess();

            if (p != null && p.MainWindowHandle != IntPtr.Zero)
            {
                UnsafeNativeMethods.ShowWindow(
                    p.MainWindowHandle, 
                    NativeMethods.ShowWindowCommands.ShowNoActivate);
                return;
            }

            Try.Invoke(async () =>
            {
                _process = null;
                ProcessUtility.StartNoActivate(KakaoEnvironment.Path);
                _process = await GetProcessAsync();
            }).Wait();
        }

        public static async Task OpenAsync() => await Task.Run(Open);

        public static void Close()
        {
            if (!CheckPathValid())
                return;

            Try.Invoke(() =>
            {
                GetProcess()?.CloseMainWindow();
                _process?.WaitForExit();
            });
        }

        public static async Task CloseAsync() => await Task.Run(Close);

        public static void Kill()
        {
            if (!CheckPathValid())
                return;

            Try.Invoke(() =>
            {
                GetProcess()?.Kill();
                _process?.WaitForExit();
                _process = null;
            });
        }

        public static async Task KillAsync() => await Task.Run(Kill);

        public static Process GetProcess()
        {
            if (!CheckPathValid())
                return null;

            if (!CheckProcessIdle())
            {
                _process = Process.GetProcesses()
                    .FirstOrDefault(p => Try.Invoke(() =>
                    {
                        return KakaoEnvironment.Path.Equals(p.MainModule.FileName);
                    }));
            }

            return _process;
        }

        public static async Task<Process> GetProcessAsync() => await Task.Run(GetProcess);

        public static KakaoMainWindow GetMainWindow()
        {
            return Window.GetChildWindows(IntPtr.Zero)
                .Where(w => Try.Invoke(() => IsKakaoWindow(w)))
                .Select(w => new KakaoMainWindow(w.Handle))
                .FirstOrDefault();
        }

        static bool IsKakaoWindow(Window w)
        {
            if (w.Class != mainWindowClassName)
                return false;

            SafeNativeMethods.GetWindowThreadProcessId(w.Handle, out int processId);
            var p = Process.GetProcessById(processId);

            if (p == null)
                return false;

            if (!KakaoEnvironment.Path.Equals(p.MainModule.FileName))
                return false;

            return w.Title == "카카오톡";
        }

        static bool CheckProcessIdle()
        {
            if (_process == null)
                return false;

            return !_process.HasExited;
        }

        static bool CheckPathValid()
        {
            return KakaoEnvironment.Path != null;
        }
    }
}
