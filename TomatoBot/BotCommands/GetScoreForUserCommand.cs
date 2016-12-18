using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;
using System.Linq;

namespace TomatoBot.BotCommands
{
    public sealed class GetScoreForUserCommand : PersonalBotCommandBase
    {
        public GetScoreForUserCommand(ScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }
        
        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && CommandUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponce(Activity activity)
        {
            var matchingGroupds = CommandUserRegex.Match(activity.Text);
            var userInfo = matchingGroupds.Groups[matchingGroupds.Groups.Count - 1].ToString();

            var userScore = _scoreRepository.GetScoreForUser(activity.Conversation.Id, userInfo);

            if (userScore != null)
            {
                var totalScore = _scoreRepository.GetScoresInConversation(activity.Conversation.Id)
                    .Sum(score => score.Score);

                return $"{userScore.PersonalScore()}, У всех остальных {totalScore - userScore.Score}";
            }

            return string.Empty;
        }

        private readonly ScoreRepository _scoreRepository;
        private static readonly Regex CommandUserRegex =
            new Regex("(/|(@GreatTomatoBot ))((счет)|(score)) @?([a-zA-Z0-9]+)");
    }
}