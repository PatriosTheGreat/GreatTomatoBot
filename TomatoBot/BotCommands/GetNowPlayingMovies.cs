using System.Linq;
using TomatoBot.Model;
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

        public bool CanExecute(MessageActivity activity) => activity.Message.StartsWith("/moviesplaying");

        public string ExecuteAndGetResponse(MessageActivity activity) =>
            string.Join(
                ActivityExtension.NewLine,
                _movieRepository.GetNowPlaying().Results.Select(movie => $"\"{movie.Title}\" Рейтинг: {movie.VoteAverage} {Emojis.GetFlag(movie.OriginalLanguage)}"));
        
        private readonly MovieRepository _movieRepository;
    }
}