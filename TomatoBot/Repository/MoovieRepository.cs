using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using TomatoBot.Model.Moovies;

namespace TomatoBot.Repository
{
    public sealed class MoovieRepository
    {
        public MoovieCollection GetNowPlaying()
        {
            return GetMooviesByMethod("now_playing");
        }

        public MoovieCollection GetUpcomingPlaying()
        {
            return GetMooviesByMethod("upcoming");
        }

        private static MoovieCollection GetMooviesByMethod(string methodName)
        {
            using (var client = new HttpClient())
            {
                var resultWorld = client.GetStringAsync(GetUrlForMethod(methodName)).Result;
                return JsonConvert.DeserializeObject<MoovieCollection>(resultWorld);
            }
        }

        private static string GetUrlForMethodWithRussiaRegion(string methodName)
        {
            return GetUrlForMethod(methodName) + "&region=ru";
        }

        private static string GetUrlForMethod(string methodName)
        {
            return $"{ApiBasePath}/{methodName}?api_key={ConfigurationManager.AppSettings["TheMovieDbKey"]}&language=ru-RU&region=ru";
        }

        private const string ApiBasePath = "https://api.themoviedb.org/3/movie";
    }
}