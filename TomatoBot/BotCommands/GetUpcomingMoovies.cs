using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class GetUpcomingMoovies : IBotCommand, ICommandWithHelpLine
    {
        public GetUpcomingMoovies(MoovieRepository moovieRepository)
        {
            _moovieRepository = moovieRepository;
        }

        public string CommandName => "getUpcomingMoovies";

        public string Description => "отображает фильмы, которые скоро выйдут в кино";

        public string Sample => "/mooviesupcoming";

        public bool CanExecute(Activity activity) => activity.Text.StartsWith("/mooviesupcoming");

        public string ExecuteAndGetResponse(Activity activity)
        {
            var moovies = _moovieRepository.GetUpcomingPlaying();

            return string.Join(
                ActivityExtension.NewLine,
                moovies.Results.Select(moovie => $"\"{moovie.Title}\" Дата выхода: {moovie.ReleaseDate} {Emojies.GetFlag(moovie.OriginalLanguage)}"));
        }

        private readonly MoovieRepository _moovieRepository;
    }
}