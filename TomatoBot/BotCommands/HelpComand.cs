using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public sealed class HelpComand : IBotCommand, ICommandWithHelpLine
    {
        public HelpComand(params ICommandWithHelpLine[] commandWithHelp)
        {
            _commandWithHelp = commandWithHelp.Concat(new[] { this }).ToArray();
        }

        public string CommandName => "help";

        public string Description => "вывод всех комманд";

        public string Sample => "/help";

        public bool CanExecute(MessageActivity activity)
        {
            return activity.Message.StartsWith("/help");
        }

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
            var title = $"Список активных команд для бота:{ActivityExtension.NewLine}";
            return title + 
                string.Join(
                    ActivityExtension.NewLine,
                    _commandWithHelp.Select(
                        command => 
                            command.CommandName + ": " + command.Description + ". Пример: " + command.Sample));
        }

        private readonly ICommandWithHelpLine[] _commandWithHelp;
    }
}