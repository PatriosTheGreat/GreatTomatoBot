using System.Linq;
using TomatoBot.Model;
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

        public bool CanExecute(MessageActivity activity) => activity.Message.StartsWith("/moviesupcoming");

        public string ExecuteAndGetResponse(MessageActivity activity) => 
            string.Join(
                ActivityExtension.NewLine,
                _movieRepository.GetUpcomingPlaying().Results.Select(moovie => $"\"{moovie.Title}\" Дата выхода: {moovie.ReleaseDate} {Emojis.GetFlag(moovie.OriginalLanguage)}"));
        
        private readonly MovieRepository _movieRepository;
    }
}