using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public static class ActivityExtension
    {
        public static bool IsMessageForBot(this Activity activity)
        {
            return activity.Text.StartsWith("/") || activity.Text.Contains(BotName);
        }
        
        public static IEnumerable<string> GetUrlsInMessage(string messages)
        {
            var matches = UrlRegex.Matches(messages);
            return from object match in matches select match.ToString();
        }

        public static bool IsUrl(string word)
        {
            return UrlRegex.IsMatch(word);
        }

        public static string UserNameRegex = "@?([_a-zA-Z0-9]+)";

        public static string NewLine = "\n\n";

        private static readonly Regex UrlRegex = new Regex(
            @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?",
            RegexOptions.Compiled);

        public const string BotName = "GreatTomatoBot";
    }
}