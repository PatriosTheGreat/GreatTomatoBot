using System.Collections.Concurrent;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.Repository
{
    public sealed class ScoreRepository
    {
        public ScoreRepository()
        {
            _azureFileDataManager = new AzureFileDataManager<MemberScore>(DataFileName);
            _scores = new ConcurrentBag<MemberScore>(_azureFileDataManager.LoadData());
        }

        public void SetScoreForUser(string conversationId, string userId, int newScore)
        {
            var userScore = GetUserScore(conversationId, userId);
            if (userScore != null)
            {
                userScore.Score = newScore;
                _azureFileDataManager.SaveData(_scores.ToArray());
            }
        }
        
        public MemberScore GetScoreForUser(string conversationId, string userInfo)
        {
            return _scores.FirstOrDefault(
                score => score.ConversationId == conversationId && (score.UserFirstName == userInfo || score.UserNickname == userInfo));
        }

        public MemberScore[] GetScoresInConversation(string conversationId)
        {
            return _scores.Where(score => score.ConversationId == conversationId).ToArray();
        }

        public void UpdateUserData(string conversationId, string userId, string userName, string userNickname)
        {
            var userScore = GetUserScore(conversationId, userId);
            if (userScore != null && (userScore.UserFirstName != userName || userScore.UserNickname != userNickname))
            {
                userScore.UserFirstName = userName;
                userScore.UserNickname = userNickname;
                _azureFileDataManager.SaveData(_scores.ToArray());
            }
            else if(userScore == null)
            {
                _scores.Add(new MemberScore
                {
                    ConversationId = conversationId,
                    Score = 0,
                    UserFirstName = userName,
                    UserNickname = userNickname,
                    UserId = userId
                });

                _azureFileDataManager.SaveData(_scores.ToArray());
            }
        }

        public bool IsUserExists(string conversationId, string userNameOrNickname)
        {
            return _scores.FirstOrDefault(
                score => 
                    score.ConversationId == conversationId && 
                    (score.UserFirstName == userNameOrNickname || score.UserNickname == userNameOrNickname)) != null;
        }

        private MemberScore GetUserScore(string conversationId, string userId)
        {
            return _scores.FirstOrDefault(score => score.ConversationId == conversationId && score.UserId == userId);
        }

        private readonly ConcurrentBag<MemberScore> _scores;
        private readonly AzureFileDataManager<MemberScore> _azureFileDataManager;
        private const string DataFileName = "tomatoscoredata";
    }
}