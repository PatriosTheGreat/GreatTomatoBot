using System.Linq;
using System.Data.SqlClient;
using TomatoBot.Model;

namespace TomatoBot.Repository
{
	public class UsersRepository : DbRepository
	{
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

		public Users GetUser(string conversationId, string userInfo)
		{
			var sqlCommand = $"{GetSelectUsersCommand()} {GetFilterByConversationIdCommand()} and ({GetFilterByUserInfo()})";

			return ExecuteReader(
				sqlCommand, 
				MapReaderToUser, 
				parametes => FillUserInformationParameters(parametes, conversationId, userInfo, userInfo, userInfo)).FirstOrDefault();
		}

		public Users GetUserById(int id) =>
			ExecuteReader(
				$"{GetSelectUsersCommand()} where {nameof(Users.Id)} = @{nameof(Users.Id)}",
				MapReaderToUser,
				parametes => FillIntParameter(parametes, nameof(Users.Id), id)).FirstOrDefault();

		public Users[] GetScoresInConversation(string conversationId) => 
			ExecuteReader(
				$"{GetSelectUsersCommand()} {GetFilterByConversationIdCommand()}", 
				MapReaderToUser,
				parametes => FillStringParameter(parametes, nameof(Users.ConversationId), conversationId)).ToArray();

		public void UpdateUserData(string conversationId, string userId, string userName, string userNickname)
		{
			var sqlCommand = GetUser(conversationId, userId) == null ? GetInsertCommand() : GetUpdateCommand();
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
	}
}