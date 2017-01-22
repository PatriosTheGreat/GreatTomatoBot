using System.Collections.Concurrent;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.Repository
{
    public sealed class MemesRepository
    {
        public MemesRepository()
        {
            _azureFileDataManager = new AzureFileDataManager<MemesInHistory>(DataFileName);
            _memeses = new ConcurrentBag<MemesInHistory>(_azureFileDataManager.LoadData());
        }

        public MemesInHistory GetMemesOrDefault(string groupId, string memesId)
        {
            return _memeses.FirstOrDefault(meme => meme.ConversationId == groupId && meme.MemesId == memesId);
        }

        public void AddMemes(MemesInHistory memes)
        {
            _memeses.Add(memes);
            _azureFileDataManager.SaveData(_memeses.ToArray());
        }
        
        private readonly ConcurrentBag<MemesInHistory> _memeses;
        private readonly AzureFileDataManager<MemesInHistory> _azureFileDataManager;
        private const string DataFileName = "tomatodata";
    }
}