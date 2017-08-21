using System;
using TomatoBot.Repository;

namespace DataMigration
{
	class Program
	{
		static void Main(string[] args)
		{
			var userRepo = new UsersRepository();

			Console.WriteLine(userRepo.GetUserById(258).Identity);
			var usersRepository = new UsersRepository();
		//	foreach(var data in dataSet)
			{
			}
			Console.ReadLine();
		}
	}
}
