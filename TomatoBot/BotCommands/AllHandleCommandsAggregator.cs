using System.Collections.Generic;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public class AllHandleCommandsAggregator : IBotCommand
    {
        public AllHandleCommandsAggregator(params IBotCommand[] botCommands)
        {
            _botCommands = botCommands;
        }

        public bool CanExecute(MessageActivity activity) =>
            _botCommands.Any(command => command.CanExecute(activity));

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
            return string.Join(". ",
                _botCommands.Where(command => command.CanExecute(activity))
                    .Select(command => command.ExecuteAndGetResponse(activity))
                    .Where(result => !string.IsNullOrEmpty(result)));
        }

        private readonly IEnumerable<IBotCommand> _botCommands;
    }
}