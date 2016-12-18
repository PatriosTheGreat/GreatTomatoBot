using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TomatoBot.BotCommands
{
    public sealed class ChannelUserData
    {
        public ChannelUserData(string channelData)
        {
            if (!string.IsNullOrEmpty(channelData))
            {

                try
                {
                    var fromToken = JObject.Parse(channelData)
                        .SelectToken("message")
                        .SelectToken("from");

                    UserNickname = GetFromString(fromToken, "username");
                    UserFirstName = GetFromString(fromToken, "first_name");
                }
                catch (JsonException)
                {
                }
            }
        }

        public string UserNickname { get; }

        public string UserFirstName { get; }

        private static string GetFromString(JToken token, string field)
        {
            return token.SelectTokens(field).FirstOrDefault()?.ToString() ?? string.Empty;
        }
    }
}