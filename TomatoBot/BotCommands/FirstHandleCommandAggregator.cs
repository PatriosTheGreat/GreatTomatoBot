using System.Collections.Generic;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public sealed class FirstHandleCommandAggregator : IBotCommand
    {
        public FirstHandleCommandAggregator(params IBotCommand[] botCommands)
        {
            _botCommands = botCommands;
        }

        public bool CanExecute(MessageActivity activity) =>
            _botCommands.Any(bot => bot.CanExecute(activity));

        public string ExecuteAndGetResponse(MessageActivity activity) =>
            _botCommands.FirstOrDefault(bot => bot.CanExecute(activity))?.ExecuteAndGetResponse(activity) ??
            string.Empty;

        private readonly IEnumerable<IBotCommand> _botCommands;
    }
}