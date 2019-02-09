using System.Collections.Generic;
using System.Linq;

namespace KakaoCommand.Common
{
    class Command
    {
        const char commandPrefix = '-';
        public string Token { get; }

        public string[] Arguments { get; }

        public Command(string token, string[] arguments)
        {
            Token = token;
            Arguments = arguments;
        }

        public static IEnumerable<Command> Parse(string[] args) => ParseImpl(args).Reverse();

        private static IEnumerable<Command> ParseImpl(string[] args)
        {
            var buffer = new List<string>();

            for (int i = args.Length - 1; i >= 0; i--)
            {
                string arg = args[i];

                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                if (arg[0] == commandPrefix)
                {
                    string token = arg.Substring(1);

                    yield return new Command(token, buffer.ToArray());

                    buffer.Clear();
                }
                else
                {
                    buffer.Insert(0, arg);
                }
            }
        }
    }
}
