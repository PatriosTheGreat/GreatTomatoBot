using System;
using Microsoft.Bot.Connector;

namespace TomatoBot.BotCommands
{
    public sealed class RudeAnswerCommand : IBotCommand
    {
        public string ExecuteAndGetResponse(Activity activity)
        {
            return Answers[_random.Next(0, Answers.Length)];
        }

        public bool CanExecute(Activity activity)
        {
            return activity.IsMessageForBot();
        }

        private static readonly string[] Answers = 
        {
            "Твоя мама случаем не прихоится сестрой твоему папе?",
            "Хватит меня угнетать, ты теломразь!",
            "Что ты делаешь со своей жизнью?",
            "Ты же не ожидаешь от меня ответа на это?",
            "Эх, надо было пойти во флот…",
            "Начнём с того, что ты пидоглазое мудило."
        };

        private readonly Random _random = new Random();
    }
}