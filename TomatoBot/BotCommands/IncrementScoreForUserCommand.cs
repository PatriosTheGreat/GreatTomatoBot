using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Reository;

namespace TomatoBot.BotCommands
{
    public sealed class IncrementScoreForUserCommand : PersonalBotCommandBase
    {
        public IncrementScoreForUserCommand(ScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && IncrementUserRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponce(Activity activity)
        {
            var matchingGroupds = IncrementUserRegex.Match(activity.Text);
            var userInfo = matchingGroupds.Groups[matchingGroupds.Groups.Count - 1].ToString();
            var userScore = _scoreRepository.GetScoreForUser(activity.Conversation.Id, userInfo);

            if (userScore != null)
            {
                _scoreRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, userScore.Score + 1);
                return userScore.PersonalScore();
            }

            return string.Empty;
        }

        private readonly ScoreRepository _scoreRepository;
        private static readonly Regex IncrementUserRegex = 
            new Regex("(/|(@GreatTomatoBot ))((увеличить счет)|(increment score)) @?([a-zA-Z0-9]+)");
    }
}