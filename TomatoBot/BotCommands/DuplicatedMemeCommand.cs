using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Bot.Connector;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class DuplicatedMemeCommand : IBotCommand
    {
        public DuplicatedMemeCommand(MemesRepository repository)
        {
            _repository = repository;
        }

        public bool CanExecute(Activity activity) => true;

        public string ExecuteAndGetResponse(Activity activity)
        {
            var responce = string.Empty;
            foreach (var url in ActivityExtension.GetUrlsInMessage(activity.Text))
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
                    responce += $"Боян этом мемес {url} уже был сканут {boyan.SendUser} примерно в {boyan.RecieveTimeUtc:o} utc!!! ";
                }
            }

            return responce;
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
        
        private readonly MemesRepository _repository;
    }
}