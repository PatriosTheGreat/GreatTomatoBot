using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public class AllHandleCommandsAggregator : IBotCommand
    {
        public AllHandleCommandsAggregator(params IBotCommand[] botCommands)
        {
            _botCommands = botCommands;
        }

        public bool CanExecute(Activity activity) =>
            _botCommands.Any(command => command.CanExecute(activity));

        public string ExecuteAndGetResponce(Activity activity)
        {
            return string.Join(". ",
                _botCommands.Where(command => command.CanExecute(activity))
                    .Select(command => command.ExecuteAndGetResponce(activity))
                    .Where(result => !string.IsNullOrEmpty(result)));
        }

        private readonly IEnumerable<IBotCommand> _botCommands;
    }
}