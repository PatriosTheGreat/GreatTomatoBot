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
            var getTotalScoreCommand = new GetTotalConversationScoreCommand(UserRepository);
            var getScoreForUserCommand = new GetScoreForUserCommand(UserRepository);
            var setScoreForUser = new SetScoreForUserCommand(UserRepository);
            var incrementScoreForUser = new IncrementScoreForUserCommand(UserRepository);
            var decrementScoreForUser = new DecrementScoreForUserCommand(UserRepository);
            var getNowPlayingMovies = new GetNowPlayingMovies(MovieRepository);
            var getUpcomingMovies = new GetUpcomingMovies(MovieRepository);
            var getMovieReleaseDate = new GetMovieReleaseDate(MovieRepository);

            _botCommands = new AllHandleCommandsAggregator(
                new DuplicatedMemeCommand(MemesRepository, UserRepository),
                new UpdateUserDataCommand(UserRepository),
                new FirstHandleCommandAggregator(
                    getCurrencyCommand,
                    getScoreForUserCommand,
                    getTotalScoreCommand,
                    setScoreForUser,
                    incrementScoreForUser,
                    decrementScoreForUser,
                    getNowPlayingMovies,
                    getUpcomingMovies,
                    getMovieReleaseDate,
                    new DetermineWrongLayoutCommand(UserRepository),
                    new HelpComand(
                        getCurrencyCommand,
                        getScoreForUserCommand,
                        getTotalScoreCommand,
                        setScoreForUser,
                        incrementScoreForUser,
                        decrementScoreForUser,
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
        private static readonly MemesesRepository MemesRepository = new MemesesRepository();
		private static readonly UsersRepository UserRepository = new UsersRepository();
		private static readonly MovieRepository MovieRepository = new MovieRepository();
    }
}