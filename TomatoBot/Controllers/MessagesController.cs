using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using TomatoBot.BotCommands;
using TomatoBot.Repository;

namespace TomatoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public MessagesController()
        {
            var getCurrencyCommand = new GetCurrencyCommand();
            var getTotalScoreCommand = new GetTotalConversationScoreCommand(ScoreRepository);
            var getScoreForUserCommand = new GetScoreForUserCommand(ScoreRepository);
            var setScoreForUser = new SetScoreForUserCommand(ScoreRepository);
            var incrementScoreForUser = new IncrementScoreForUserCommand(ScoreRepository);
            var getNowPlayingMovies = new GetNowPlayingMovies(MovieRepository);
            var getUpcomingMovies = new GetUpcomingMovies(MovieRepository);
            var getMovieReleaseDate = new GetMovieReleaseDate(MovieRepository);

            _botCommands = new AllHandleCommandsAggregator(
                new DuplicatedMemeCommand(MemesRepository),
                new UpdateUserDataCommand(ScoreRepository),
                new FirstHandleCommandAggregator(
                    getCurrencyCommand,
                    getScoreForUserCommand,
                    getTotalScoreCommand,
                    setScoreForUser,
                    incrementScoreForUser,
                    getNowPlayingMovies,
                    getUpcomingMovies,
                    getMovieReleaseDate,
                    new DetermineWrongLayoutCommand(ScoreRepository),
                    new HelpComand(
                        getCurrencyCommand,
                        getScoreForUserCommand,
                        getTotalScoreCommand,
                        setScoreForUser,
                        incrementScoreForUser,
                        getNowPlayingMovies,
                        getUpcomingMovies,
                        getMovieReleaseDate),
                    new RudeAnswerCommand()));
        }

        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message && !string.IsNullOrEmpty(activity.Text))
            {
                var responce = _botCommands.ExecuteAndGetResponse(activity);

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
        private static readonly MovieRepository MovieRepository = new MovieRepository();
    }
}