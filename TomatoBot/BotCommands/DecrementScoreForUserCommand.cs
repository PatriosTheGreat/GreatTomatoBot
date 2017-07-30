﻿using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class DecrementScoreForUserCommand : ScoreCommandBase, ICommandWithHelpLine
    {
        public DecrementScoreForUserCommand(UsersRepository usersRepository) : base(usersRepository)
        {
        }

        public string CommandName => "decrementScoreForUser";

        public string Description => "уменьшает счет определенному участнику чата";

        public string Sample => "@GreatTomatoBot @UserName -";

        public override bool CanExecute(Activity activity)
        {
            return base.CanExecute(activity) && ValidateIncrementUserCommandRegex.IsMatch(activity.Text);
        }

        public override string ExecuteAndGetResponse(Activity activity)
        {
            var userScore = GetScoreForUserOrNull(activity);

            if (userScore != null)
            {
				UserRepository.SetScoreForUser(activity.Conversation.Id, userScore.UserId, userScore.Score - 1);
                return userScore.PersonalScore();
            }

            return string.Empty;
        }
        
        private static readonly Regex ValidateIncrementUserCommandRegex = 
            new Regex($"(/|(@GreatTomatoBot)) (((уменьшить счет)|(decrement score)) {ActivityExtension.UserNameRegex}|{ActivityExtension.UserNameRegex} [-])");
    }
}