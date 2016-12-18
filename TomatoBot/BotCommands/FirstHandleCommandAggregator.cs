using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public sealed class FirstHandleCommandAggregator : IBotCommand
    {
        public FirstHandleCommandAggregator(params IBotCommand[] botCommands)
        {
            _botCommands = botCommands;
        }

        public bool CanExecute(Activity activity) =>
            _botCommands.Any(bot => bot.CanExecute(activity));

        public string ExecuteAndGetResponce(Activity activity) =>
            _botCommands.FirstOrDefault(bot => bot.CanExecute(activity))?.ExecuteAndGetResponce(activity) ??
            string.Empty;

        private readonly IEnumerable<IBotCommand> _botCommands;
    }
}