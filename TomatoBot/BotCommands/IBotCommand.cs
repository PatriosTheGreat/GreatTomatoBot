using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public interface IBotCommand
    {
        bool CanExecute(Activity activity);

        string ExecuteAndGetResponse(Activity activity);
    }
}
