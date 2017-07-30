using System;
using System.Linq;
using TomatoBot.Model;
using TomatoBot.Repository;

namespace DataMigration
{
	class Program
	{
		static void Main(string[] args)
		{
			const string DataFileName = "tomatoscoredata";
			AzureFileDataManager<MemberScore> azureFileDataManager = new AzureFileDataManager<MemberScore>(DataFileName);

			var dataSet = azureFileDataManager.LoadData();
			Console.WriteLine(dataSet.Count());

			var usersRepository = new UsersRepository();
			foreach(var data in dataSet)
			{
				if (string.IsNullOrEmpty(data.UserId))
				{
					Console.WriteLine(data.UserNickname);
					continue;
				}

				usersRepository.UpdateUserData(data.ConversationId, data.UserId, data.UserFirstName, data.UserNickname);
				usersRepository.SetScoreForUser(data.ConversationId, data.UserId, data.Score);

				if(usersRepository.IsUserExists(data.ConversationId, data.UserId))
				{
					Console.WriteLine($"All cool with {data.UserId} with score {usersRepository.GetScoreForUser(data.ConversationId, data.UserNickname)}");
				}
				else
				{
					break;
				}
			}

			Console.ReadLine();
		}
	}
}
