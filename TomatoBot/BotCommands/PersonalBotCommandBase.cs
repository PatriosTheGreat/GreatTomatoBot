using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public abstract class PersonalBotCommandBase : IBotCommand
    {
        public virtual bool CanExecute(Activity activity) =>
            activity.Text.StartsWith("/") || activity.Text.Contains(BotName);

        public abstract string ExecuteAndGetResponce(Activity activity);

        public const string BotName = "GreatTomatoBot";
    }
}