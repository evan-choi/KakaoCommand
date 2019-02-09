using KakaoCommand.Common;

namespace KakaoCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example)
            // .\KakaoCommand.exe -open
            // .\KakaoCommand.exe -open <친구 이름>

            var commandSet = new CommandSet()
            {
                new CommandAction("open", 0, Open),
                new CommandAction("friend", 1, Friend),
            };

            commandSet.Run(args);
        }

        private static void Open(Command obj)
        {
            Kakao.Open();
        }

        private static void Friend(Command obj)
        {
            string name = obj.Arguments[0];

            if (string.IsNullOrEmpty(name))
                return;

            var window = Kakao.GetMainWindow();

            if (window == null)
            {
                Kakao.Open();
                return;
            }

            window.OpenChatRoom(name);
        }
    }
}
