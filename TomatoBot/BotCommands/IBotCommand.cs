using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public interface IBotCommand
    {
        bool CanExecute(MessageActivity activity);

        string ExecuteAndGetResponse(MessageActivity activity);
    }
}
