using System;
using System.Security.Cryptography;
using System.Text;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace TomatoBot.BotCommands
{
    public sealed class DuplicatedMemeCommand : IBotCommand
    {
        public DuplicatedMemeCommand(MemesesRepository repository, UsersRepository usersRepository)
        {
            _repository = repository;
			_userRepository = usersRepository;
        }

        public bool CanExecute(MessageActivity activity) => true;

        public string ExecuteAndGetResponse(MessageActivity activity)
        {
            var responce = string.Empty;
            foreach (var url in activity.Links)
            {
                var urlId = GetSha256(new Uri(url).ToString());
                var boyan = _repository.GetMemesOrDefault(activity.ConversationId, urlId);

                if (boyan == null)
                {
					_repository.AddMemes(
                        new Memeses(
							0,
							DateTime.UtcNow,
							activity.FromUser.Id,
							urlId,
							activity.ConversationId));
                }
                else
				{
					var user = _userRepository.GetUserById(boyan.UserId);
					if (user != null)
					{
						responce += $"Боян этом мемес {url} уже был скинут {user.Identity} примерно в {boyan.SendTime} utc!!! ";
					}
				}
            }

            return responce;
        }

        private static string GetSha256(string value)
        {
            var stringBuilder = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (var @byte in result)
                {
                    stringBuilder.Append(@byte.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }
        
        private readonly MemesesRepository _repository;
		private readonly UsersRepository _userRepository;
	}
}