using System;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace DataMigration
{
	class Program
	{
		static void Main(string[] args)
		{
			const string DataFileName = "tomatodata";
			var azureFileDataManager = new AzureFileDataManager<MemesInHistory>(DataFileName);

			var memeses = azureFileDataManager.LoadData();
			/*
			var memesesRepo = new MemesesRepository();
			var userRepo = new UsersRepository();
			foreach (var memes in memeses)
			{
				var user = userRepo.GetUser(memes.ConversationId, memes.SendUser);
				if(user == null)
				{
					continue;
				}

				memesesRepo.AddMemes(new Memeses(0, memes.RecieveTimeUtc, user.Id, memes.MemesId, memes.ConversationId));

				if (memesesRepo.GetMemesOrDefault(memes.ConversationId, memes.MemesId) == null)
				{
					Console.WriteLine("Huston we have a promlem!");
					break;
				}
			}*/
			var userRepo = new UsersRepository();

			Console.WriteLine(userRepo.GetUserById(258).Identity);
			/*
			AzureFileDataManager<MemberScore> azureFileDataManager = new AzureFileDataManager<MemberScore>(DataFileName);

			var dataSet = azureFileDataManager.LoadData().Where(d => d.ConversationId == "-1001062731823").ToArray();
			Console.WriteLine(dataSet.Count());

			var usersRepository = new UsersRepository();
			foreach(var data in dataSet)
			{
				if (string.IsNullOrEmpty(data.UserId))
				{
					Console.WriteLine(data.UserNickname);
					continue;
				}

				if (!usersRepository.IsUserExists(data.ConversationId, data.UserId))
				{
					usersRepository.UpdateUserData(data.ConversationId, data.UserId, data.UserFirstName, data.UserNickname);
					usersRepository.SetScoreForUser(data.ConversationId, data.UserId, data.Score);
				}
				/*
				if(usersRepository.IsUserExists(data.ConversationId, data.UserId))
				{
					Console.WriteLine($"All cool with {data.UserId} with score {usersRepository.GetScoreForUser(data.ConversationId, data.UserNickname)}");
				}
				else
				{
					break;
				}
			}
		*/
			Console.ReadLine();
		}
	}
}
