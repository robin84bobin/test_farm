using System;
using System.Linq;

namespace Commands
{
    public class CommandSequence : Command
    {
        private readonly Command[] _commands;

        /// <summary>
        /// do sequence of commands
        /// </summary>
        /// <param name="commands"></param>
        public CommandSequence(params Command[] commands)
        {
            _commands = commands;
        }
        

        public override void Execute()
        {
            for (var i = 0; i < _commands.Length; i++)
            {
                if (i < _commands.Length - 1)
                {
                    var nextCommand = _commands[i + 1];
                    var currentCommand = i;
                    _commands[i].OnComplete += () =>
                    {
                        CommandManager.Execute(nextCommand);
                    };
                }
                else if (i == _commands.Length - 1)
                    _commands[i].OnComplete += () =>
                    {
                        Complete();
                    };
                    
            }

            CommandManager.Execute(_commands.First());
        }

        protected override void Complete()
        {
            base.Complete();
        }

    }
}