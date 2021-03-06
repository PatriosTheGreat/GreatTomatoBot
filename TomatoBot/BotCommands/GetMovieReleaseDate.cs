﻿using System;
using System.Linq;
using TomatoBot.Model;
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
        
        public bool CanExecute(MessageActivity activity) => activity.IsMessageForBot() && activity.Message.Contains(ReleaseDate);
        
        public string ExecuteAndGetResponse(MessageActivity activity)
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
                movies.Results.Take(MaxMoviesCount).OrderByDescending(movie => movie.ReleaseDate).Select(movie => $"{movie.Title} {movie.ReleaseDate}"));
        }
        
        private static string GetMovieName(MessageActivity activity)
        {
            var index = activity.Message.IndexOf(ReleaseDate, StringComparison.Ordinal);
            if (index == -1)
            {
                return string.Empty;
            }

            return activity.Message.Substring(index + ReleaseDate.Length).Trim();
        }

        private const int MaxMoviesCount = 5;
        private readonly MovieRepository _movieRepository;
        private const string UnknownMovie = "Понятия не имею, что это за фильм";
        private const string ReleaseDate = "дата выхода";
    }
}