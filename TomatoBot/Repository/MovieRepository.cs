using System.Configuration;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using TomatoBot.Model.Movies;

namespace TomatoBot.Repository
{
    public sealed class MovieRepository
    {
        public MovieCollection GetNowPlaying()
        {
            return GetMooviesByMethod("movie/now_playing", "&region=RU");
        }

        public MovieCollection GetUpcomingPlaying()
        {
            return GetMooviesByMethod("movie/upcoming", "&region=RU");
        }

        public MovieCollection SearchMoovie(string query)
        {
            return GetMooviesByMethod("search/movie", $"&query={HttpUtility.HtmlEncode(query)}");
        }

        private static MovieCollection GetMooviesByMethod(string methodName, string additionalParameters)
        {
            using (var client = new HttpClient())
            {
                var resultWorld = client.GetStringAsync(GetUrlForMethod(methodName, additionalParameters)).Result;
                return JsonConvert.DeserializeObject<MovieCollection>(resultWorld);
            }
        }

        private static string GetUrlForMethod(string methodName, string additionalParameters)
        {
            return $"{ApiBasePath}/{methodName}?api_key={ConfigurationManager.AppSettings["TheMovieDbKey"]}&language=ru-RU{additionalParameters}";
        }

        private const string ApiBasePath = "https://api.themoviedb.org/3";
    }
}