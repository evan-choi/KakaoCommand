using System;

namespace KakaoCommand.Common
{
    class CommandAction
    {
        public string Token { get; }

        int _argumentCount;
        Action<Command> _action;

        public CommandAction(string token, int argCount, Action<Command> action)
        {
            Token = token;
            _argumentCount = argCount;
            _action = action;
        }

        public bool Execute(Command command)
        {
            if (!string.Equals(Token, command.Token, StringComparison.OrdinalIgnoreCase))
                return false;

            if (command.Arguments.Length < _argumentCount)
                return false;

            _action.Invoke(command);
            return true;
        }
    }
}
