using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class GetNowPlayingMovies : IBotCommand, ICommandWithHelpLine
    {
        public GetNowPlayingMovies(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public string CommandName => "moviesPlaying";

        public string Description => "отображает фильмы, которые сейчас идут в кино";

        public string Sample => "/moviesplaying";

        public bool CanExecute(Activity activity) => activity.Text.StartsWith("/moviesplaying");

        public string ExecuteAndGetResponse(Activity activity) =>
            string.Join(
                ActivityExtension.NewLine,
                _movieRepository.GetNowPlaying().Results.Select(movie => $"\"{movie.Title}\" Рейтинг: {movie.VoteAverage} {Emojis.GetFlag(movie.OriginalLanguage)}"));
        
        private readonly MovieRepository _movieRepository;
    }
}