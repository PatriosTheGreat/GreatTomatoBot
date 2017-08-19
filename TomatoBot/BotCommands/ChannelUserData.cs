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
	                var messageToken = JObject.Parse(channelData).SelectToken("message");
					var fromToken = messageToken.SelectToken("from");

                    UserNickname = GetFromString(fromToken, "username");
                    UserFirstName = GetFromString(fromToken, "first_name");

					var replyToken = messageToken.SelectToken("reply_to_message")?.SelectToken("from");
	                if (replyToken != null)
	                {
						ReplyToId = GetFromString(replyToken, "id");
					}
				}
                catch (JsonException)
                {
                }
			}
        }

        public string UserNickname { get; }

        public string UserFirstName { get; }

		public string ReplyToId { get; }

        private static string GetFromString(JToken token, string field)
        {
            return token.SelectTokens(field).FirstOrDefault()?.ToString() ?? string.Empty;
        }
    }
}