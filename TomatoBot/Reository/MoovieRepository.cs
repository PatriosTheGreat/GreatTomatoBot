using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using TomatoBot.Model.Moovies;

namespace TomatoBot.Reository
{
    public sealed class MoovieRepository
    {
        public MoovieCollection GetNowPlaying()
        {
            using (var client = new HttpClient())
            {
                var result = client.GetStringAsync(GetUrlForMethod("now_playing")).Result;
                return JsonConvert.DeserializeObject<MoovieCollection>(result);
            }
        }

        private static string GetUrlForMethod(string methodName)
        {
            return $"{ApiBasePath}/{methodName}?api_key={ConfigurationManager.AppSettings["TheMovieDbKey"]}&language=ru-RU";
        }

        private const string ApiBasePath = "https://api.themoviedb.org/3/movie";
    }
}