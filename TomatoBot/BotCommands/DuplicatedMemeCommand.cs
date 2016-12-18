using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Model;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class DuplicatedMemeCommand : IBotCommand
    {
        public DuplicatedMemeCommand(MemesRepository repository)
        {
            _repository = repository;
        }

        public bool CanExecute(Activity activity) => true;

        public string ExecuteAndGetResponce(Activity activity)
        {
            var responce = string.Empty;
            foreach (var url in GetUrlsInMessage(activity.Text))
            {
                var urlId = GetSha256(new Uri(url).ToString());
                var boyan = _repository.GetMemesOrDefault(activity.Conversation.Id, urlId);

                if (boyan == null)
                {
                    var userChannelData = new ChannelUserData(activity.ChannelData?.ToString());
                    var userName =
                        !string.IsNullOrEmpty(userChannelData.UserNickname)
                            ? ("@" + userChannelData.UserNickname)
                            : !string.IsNullOrEmpty(userChannelData.UserFirstName) ? userChannelData.UserFirstName : "понятия не имею кем";

                    _repository.AddMemes(
                        new MemesInHistory(
                            activity.Conversation.Id,
                            urlId,
                            DateTime.UtcNow,
                            userName));
                }
                else
                {
                    responce += $"Боян этом мемес {url} уже был сканут {boyan.SendUser} примерно в {boyan.RecieveTimeUtc.ToString("dd.mm.hh HH:MM")} utc!!! ";
                }
            }

            return responce;
        }

        private static IEnumerable<string> GetUrlsInMessage(string messages)
        {
            var matches = UrlRegex.Matches(messages);
            return from object match in matches select match.ToString();
        }

        private static string GetSha256(string value)
        {
            var stringBuilder = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (var @byte in result)
                {
                    stringBuilder.Append(@byte.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }
        
        private static Regex UrlRegex = new Regex(
            @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?",
            RegexOptions.Compiled);

        private readonly MemesRepository _repository;
    }
}