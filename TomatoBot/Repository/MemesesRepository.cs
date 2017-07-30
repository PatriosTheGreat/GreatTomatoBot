using System.Data.SqlClient;
using System.Linq;
using TomatoBot.Model;

namespace TomatoBot.Repository
{
	public class MemesesRepository : DbRepository
	{
		public Memeses GetMemesOrDefault(string conversationId, string memesHash)
		{
			var queryString = $"select * from {nameof(Memeses)} where {nameof(Memeses.ConversationId)} = @{nameof(Memeses.ConversationId)} and {nameof(Memeses.Hash)} = @{nameof(Memeses.Hash)}";

			return ExecuteReader(
				queryString,
				MapReaderToMemes,
				collection =>
				{
					FillStringParameter(collection, nameof(Memeses.ConversationId), conversationId);
					FillStringParameter(collection, nameof(Memeses.Hash), memesHash);
				}).FirstOrDefault();
		}

		public void AddMemes(Memeses memes)
		{
			var query = $"insert into {nameof(Memeses)} " +
				$"({nameof(Memeses.SendTime)}, {nameof(Memeses.UserId)}, {nameof(Memeses.Hash)}, {nameof(Memeses.ConversationId)}) values " +
				$"(@{nameof(Memeses.SendTime)}, @{nameof(Memeses.UserId)}, @{nameof(Memeses.Hash)}, @{nameof(Memeses.ConversationId)})";

			ExecuteNonQuery(
				query,
				collection =>
				{
					FillStringParameter(collection, nameof(Memeses.ConversationId), memes.ConversationId);
					FillStringParameter(collection, nameof(Memeses.Hash), memes.Hash);
					FillIntParameter(collection, nameof(Memeses.UserId), memes.UserId);
					FillDateTimeParameter(collection, nameof(Memeses.SendTime), memes.SendTime);
				});
		}

		private Memeses MapReaderToMemes(SqlDataReader reader) =>
			new Memeses(
				reader.GetInt32(reader.GetOrdinal(nameof(Memeses.Id))),
				reader.GetDateTime(reader.GetOrdinal(nameof(Memeses.SendTime))),
				reader.GetInt32(reader.GetOrdinal(nameof(Memeses.UserId))),
				reader.GetString(reader.GetOrdinal(nameof(Memeses.Hash))),
				reader.GetString(reader.GetOrdinal(nameof(Memeses.ConversationId))));
	}
}