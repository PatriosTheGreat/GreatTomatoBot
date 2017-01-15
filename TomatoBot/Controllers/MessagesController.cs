using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using TomatoBot.BotCommands;
using TomatoBot.Reository;

namespace TomatoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public MessagesController()
        {
            _botCommands = new AllHandleCommandsAggregator(
                new DuplicatedMemeCommand(MemesRepository),
                new UpdateUserDataCommand(ScoreRepository),
                new FirstHandleCommandAggregator(
                    new GetCurrencyCommand(),
                    new GetTotalConversationScoreCommand(ScoreRepository),
                    new IncrementScoreForUserCommand(ScoreRepository),
                    new GetScoreForUserCommand(ScoreRepository),
                    new SetScoreForUserCommand(ScoreRepository),
                    new DetermineWrongLayoutCommand(ScoreRepository),
                    new RudeAnswerCommand()));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(activity.Text))
            {
                var responce = _botCommands.ExecuteAndGetResponce(activity);

                if (!string.IsNullOrEmpty(responce))
                {
                    await new ConnectorClient(new Uri(activity.ServiceUrl))
                        .Conversations
                        .ReplyToActivityAsync(activity.CreateReply(responce));
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private readonly IBotCommand _botCommands;
        private static readonly MemesRepository MemesRepository = new MemesRepository();
        private static readonly ScoreRepository ScoreRepository = new ScoreRepository();
    }
}