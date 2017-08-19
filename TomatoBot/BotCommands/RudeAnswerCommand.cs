using System;
using System.Collections.Generic;
using TomatoBot.Model;

namespace TomatoBot.BotCommands
{
    public sealed class RudeAnswerCommand : IBotCommand
    {
        public string ExecuteAndGetResponse(MessageActivity activity)
        {
            var index = (GetLastAnswer(activity.ConversationId) + _random.Next(0, Answers.Length - 1) + 1) % Answers.Length;
            return Answers[index];
        }

        public bool CanExecute(MessageActivity activity)
        {
            return activity.IsMessageForBot();
        }

        private int GetLastAnswer(string conversationId)
        {
            if (_conversationToLastAnswer.ContainsKey(conversationId))
            {
                return _conversationToLastAnswer[conversationId];
            }

            _conversationToLastAnswer.Add(conversationId, 0);
            return 0;
        }

        private static readonly string[] Answers = 
        {
            "Твоя мама случаем не прихоится сестрой твоему папе?",
            "Хватит меня угнетать, ты теломразь!",
            "Что ты делаешь со своей жизнью?",
            "Ты же не ожидаешь от меня ответа на это?",
            "Эх, надо было пойти во флот…",
            "Может лучше книжку почитаешь?",
            "Надеюсь ты этим не в рабочее время занят.",
            "Твои родители случаем не выписывали журнал \"Здоровье\"?",
            "Вот с такими как ты мне и приходится общаться =(",
            "На небе только и разговоров, что о море и о закате.",
            "В очередь, сукины дети!",
			"А часики-то тикают...",
			"Чик-чирик"
        };

        private readonly Dictionary<string, int> _conversationToLastAnswer = new Dictionary<string, int>();
        private readonly Random _random = new Random();
    }
}