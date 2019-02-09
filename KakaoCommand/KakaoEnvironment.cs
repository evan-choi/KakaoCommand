using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace KakaoCommand
{
    static class KakaoEnvironment
    {
        public static string Path { get; }

        static KakaoEnvironment()
        {
            Path = GetKakaoPath();
        }

        static string GetKakaoPath()
        {
            try
            {
                var reg = Registry.ClassesRoot.OpenSubKey(@"kakaoopen\shell\open\command\", false);

                if (reg == null)
                    return null;

                string value = (string)reg.GetValue("");
                var match = Regex.Match(value, "(?<=\")[^\"]*(?=\")");

                if (!match.Success)
                    return null;

                return match.Value;
            }
            catch
            {
            }

            return null;
        }
    }
}
