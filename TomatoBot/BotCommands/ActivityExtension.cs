using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public static class ActivityExtension
    {
        public static bool IsAdressToBot(this Activity activity)
        {
            return activity.Text.StartsWith("/") || activity.Text.Contains(BotName);
        }

        public const string BotName = "GreatTomatoBot";
    }
}