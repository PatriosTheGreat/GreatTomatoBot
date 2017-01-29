using System;
using System.Linq;
using Microsoft.Bot.Connector;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class GetMovieReleaseDate : IBotCommand, ICommandWithHelpLine
    {
        public GetMovieReleaseDate(MovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public string CommandName => "getMovieReleaseDate";

        public string Description => "отображает дату релиза запрошенного фильма";

        public string Sample => $"@{ActivityExtension.BotName} {ReleaseDate} название фильма";
        
        public bool CanExecute(Activity activity)
        {
            return activity.IsMessageForBot() && activity.Text.Contains(ReleaseDate);
        }

        public string ExecuteAndGetResponse(Activity activity)
        {
            var query = GetMovieName(activity);
            if (string.IsNullOrWhiteSpace(query))
            {
                return UnknownMovie;
            }

            var movies = _movieRepository.SearchMoovie(query);
            if (movies.TotalResults == 0)
            {
                return UnknownMovie;
            }

            return string.Join(
                ActivityExtension.NewLine, 
                movies.Results.Take(MaxMoviesCount).OrderByDescending(movie => DateTime.Parse(movie.ReleaseDate)).Select(movie => $"{movie.Title} {movie.ReleaseDate}"));
        }
        
        private static string GetMovieName(IMessageActivity activity)
        {
            var index = activity.Text.IndexOf(ReleaseDate, StringComparison.Ordinal);
            if (index == -1)
            {
                return string.Empty;
            }

            return activity.Text.Substring(index + ReleaseDate.Length).Trim();
        }

        private const int MaxMoviesCount = 5;
        private readonly MovieRepository _movieRepository;
        private const string UnknownMovie = "Понятия не имею, что это за фильм";
        private const string ReleaseDate = "дата выхода";
    }
}