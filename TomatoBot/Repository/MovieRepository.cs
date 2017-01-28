using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using TomatoBot.Model.Movies;

namespace TomatoBot.Repository
{
    public sealed class MovieRepository
    {
        public MovieCollection GetNowPlaying()
        {
            return GetMooviesByMethod("now_playing");
        }

        public MovieCollection GetUpcomingPlaying()
        {
            return GetMooviesByMethod("upcoming");
        }

        private static MovieCollection GetMooviesByMethod(string methodName)
        {
            using (var client = new HttpClient())
            {
                var resultWorld = client.GetStringAsync(GetUrlForMethod(methodName)).Result;
                return JsonConvert.DeserializeObject<MovieCollection>(resultWorld);
            }
        }

        private static string GetUrlForMethod(string methodName)
        {
            return $"{ApiBasePath}/{methodName}?api_key={ConfigurationManager.AppSettings["TheMovieDbKey"]}&language=ru-RU&region=RU";
        }

        private const string ApiBasePath = "https://api.themoviedb.org/3/movie";
    }
}