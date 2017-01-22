using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class GetNowPlayingMoovies : IBotCommand, ICommandWithHelpLine
    {
        public GetNowPlayingMoovies(MoovieRepository moovieRepository)
        {
            _moovieRepository = moovieRepository;
        }

        public string CommandName => "mooviesPlaying";

        public string Description => "отображает фильмы, которые сейчас идут в кино";

        public string Sample => "/mooviesplaying";

        public bool CanExecute(Activity activity) => activity.Text.StartsWith("/mooviesplaying");

        public string ExecuteAndGetResponse(Activity activity)
        {
            var moovies = _moovieRepository.GetNowPlaying();

            return string.Join(
                ActivityExtension.NewLine,
                moovies.Results.Select(moovie => $"\"{moovie.Title}\" Рейтинг: {moovie.VoteAverage} {Emojies.GetFlag(moovie.OriginalLanguage)}"));
        }

        private readonly MoovieRepository _moovieRepository;
    }
}