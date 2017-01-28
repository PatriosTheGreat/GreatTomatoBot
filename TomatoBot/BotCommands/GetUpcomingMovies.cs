using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class GetUpcomingMovies : IBotCommand, ICommandWithHelpLine
    {
        public GetUpcomingMovies(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public string CommandName => "getUpcomingMovies";

        public string Description => "отображает фильмы, которые скоро выйдут в кино";

        public string Sample => "/moviesupcoming";

        public bool CanExecute(Activity activity) => activity.Text.StartsWith("/moviesupcoming");

        public string ExecuteAndGetResponse(Activity activity) => 
            string.Join(
                ActivityExtension.NewLine,
                _movieRepository.GetUpcomingPlaying().Results.Select(moovie => $"\"{moovie.Title}\" Дата выхода: {moovie.ReleaseDate} {Emojis.GetFlag(moovie.OriginalLanguage)}"));
        
        private readonly MovieRepository _movieRepository;
    }
}