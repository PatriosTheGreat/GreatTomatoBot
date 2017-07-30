using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TomatoBot.Model;
using Microsoft.Azure;

namespace TomatoBot.Repository
{
	public class UsersRepository
	{
		public UsersRepository()
		{
			_connectionString = CloudConfigurationManager.GetSetting("ConnectionString");
		}

		public void SetScoreForUser(string conversationId, string userId, int newScore)
		{
			var sqlCommand = $"update {nameof(Users)} set [{nameof(Users.Score)}] = @{nameof(Users.Score)} " +
				$"{GetFilterByConversationIdCommand()} and {nameof(Users.UserId)} = @{nameof(Users.UserId)}";

			ExecuteNonQuery(
				sqlCommand,
				parameters =>
				{
					FillStringParameter(parameters, nameof(Users.ConversationId), conversationId);
					FillStringParameter(parameters, nameof(Users.UserId), userId ?? "");
					FillIntParameter(parameters, nameof(Users.Score), newScore);
				});
		}

		public Users GetScoreForUser(string conversationId, string userInfo)
		{
			var sqlCommand = $"{GetSelectUsersCommand()} {GetFilterByConversationIdCommand()} and ({GetFilterByUserInfo()})";

			return ExecuteReader(
				sqlCommand, 
				MapReaderToUser, 
				parametes => FillUserInformationParameters(parametes, conversationId, userInfo, userInfo, userInfo)).FirstOrDefault();
		}

		public Users[] GetScoresInConversation(string conversationId) => 
			ExecuteReader(
				$"{GetSelectUsersCommand()} {GetFilterByConversationIdCommand()}", 
				MapReaderToUser,
				parametes => FillStringParameter(parametes, nameof(Users.ConversationId), conversationId)).ToArray();

		public void UpdateUserData(string conversationId, string userId, string userName, string userNickname)
		{
			var sqlCommand = GetScoreForUser(conversationId, userId) == null ? GetInsertCommand() : GetUpdateCommand();
			ExecuteNonQuery(sqlCommand, parameters => FillUserInformationParameters(parameters, conversationId, userId, userName, userNickname));
		}

		public bool IsUserExists(string conversationId, string userNameOrNickname)
		{
			var sqlCommand = $"{GetSelectUsersCommand()} {GetFilterByConversationIdCommand()} and ({GetFilterByUserInfo()})";

			return ExecuteReader(
				sqlCommand,
				MapReaderToUser,
				parametes => FillUserInformationParameters(parametes, conversationId, userNameOrNickname, userNameOrNickname, userNameOrNickname)).Any();
		}

		private string GetFilterByUserInfo() =>
			$"{nameof(Users.Nickname)} = @{nameof(Users.Nickname)} or {nameof(Users.FirstName)} = @{nameof(Users.FirstName)} or {nameof(Users.UserId)} = @{nameof(Users.UserId)}";

		private string GetSelectUsersCommand() => $"Select * from {nameof(Users)}";

		private string GetFilterByConversationIdCommand() => $"where {nameof(Users.ConversationId)} = @{nameof(Users.ConversationId)}";

		private Users MapReaderToUser(SqlDataReader reader) => 
			new Users(
				reader.GetInt32(reader.GetOrdinal(nameof(Users.Id))),
				reader.GetString(reader.GetOrdinal(nameof(Users.Nickname))),
				reader.GetString(reader.GetOrdinal(nameof(Users.FirstName))),
				reader.GetString(reader.GetOrdinal(nameof(Users.UserId))),
				reader.GetString(reader.GetOrdinal(nameof(Users.ConversationId))),
				reader.GetInt32(reader.GetOrdinal(nameof(Users.Score))));
		
		private IEnumerable<Users> ExecuteReader(string queryString, Func<SqlDataReader, Users> map, Action<SqlParameterCollection> fillParameters)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(queryString, connection);
				fillParameters(command.Parameters);
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						yield return map(reader);
					}
				}
			}
		}

		private int ExecuteNonQuery(string queryString, Action<SqlParameterCollection> fillParameters)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var command = new SqlCommand(queryString, connection);
				fillParameters(command.Parameters);
				return command.ExecuteNonQuery();
			}
		}

		private static string GetInsertCommand() =>
			$"insert into {nameof(Users)} ({nameof(Users.Nickname)}, {nameof(Users.FirstName)}, {nameof(Users.UserId)}, {nameof(Users.ConversationId)}, {nameof(Users.Score)}) " +
				$"values(@{nameof(Users.Nickname)}, @{nameof(Users.FirstName)}, @{nameof(Users.UserId)}, @{nameof(Users.ConversationId)}, 0)";

		private static string GetUpdateCommand() =>
			$"update {nameof(Users)} set [{nameof(Users.Nickname)}] = @{nameof(Users.Nickname)}, [{nameof(Users.FirstName)}] = @{nameof(Users.FirstName)}, [{nameof(Users.ConversationId)}] = @{nameof(Users.ConversationId)} " +
			$"where [{nameof(Users.UserId)}] = @{nameof(Users.UserId)}";

		private static void FillUserInformationParameters(
			SqlParameterCollection parameterCollection, 
			string conversationId, 
			string userId, 
			string userName, 
			string userNickname)
		{
			FillStringParameter(parameterCollection, nameof(Users.ConversationId), conversationId);
			FillStringParameter(parameterCollection, nameof(Users.UserId), userId ?? "");
			FillStringParameter(parameterCollection, nameof(Users.FirstName), userName ?? "");
			FillStringParameter(parameterCollection, nameof(Users.Nickname), userNickname ?? "");
		}

		private static void FillStringParameter(SqlParameterCollection parameterCollection, string parameterName, string parameterValue)
		{
			parameterCollection.Add($"@{parameterName}", SqlDbType.NVarChar);
			parameterCollection[$"@{parameterName}"].Value = parameterValue;
		}

		private static void FillIntParameter(SqlParameterCollection parameterCollection, string parameterName, int parameterValue)
		{
			parameterCollection.Add($"@{parameterName}", SqlDbType.Int);
			parameterCollection[$"@{parameterName}"].Value = parameterValue;
		}

		private readonly string _connectionString;
	}
}