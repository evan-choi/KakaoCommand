using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KakaoCommand.Common
{
    class CommandSet : IEnumerable<CommandAction>
    {
        List<CommandAction> _commandActions;

        public CommandSet() : this(Enumerable.Empty<CommandAction>())
        {
        }

        public CommandSet(IEnumerable<CommandAction> commands)
        {
            _commandActions = new List<CommandAction>(commands);
        }

        public void Add(CommandAction action)
        {
            _commandActions.Add(action);
        }

        public void Remove(CommandAction action)
        {
            _commandActions.Remove(action);
        }

        public bool Run(string[] args)
        {
            bool find = false;

            foreach (Command command in Command.Parse(args))
            {
                foreach (CommandAction action in _commandActions)
                {
                    if (action.Execute(command))
                    {
                        find = true;
                        break;
                    }
                }
            }

            return find;
        }

        public IEnumerator<CommandAction> GetEnumerator()
        {
            return _commandActions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commandActions.GetEnumerator();
        }
    }
}
