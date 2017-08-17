using TomatoBot.Model;

namespace TomatoBot.Repository
{
	public class MessagesRepository : DbRepository
	{
		public void AddMessage(Messages message)
		{
			var query = $"insert into {nameof(Messages)} " +
				$"({nameof(Messages.ConversationId)}, {nameof(Messages.UserId)}, {nameof(Messages.WordsCount)}, {nameof(Messages.Time)}) values " +
				$"(@{nameof(Messages.ConversationId)}, @{nameof(Messages.UserId)}, @{nameof(Messages.WordsCount)}, @{nameof(Messages.Time)})";

			ExecuteNonQuery(
				query,
				collection =>
				{
					FillStringParameter(collection, nameof(Messages.ConversationId), message.ConversationId);
					FillIntParameter(collection, nameof(Messages.WordsCount), message.WordsCount);
					FillStringParameter(collection, nameof(Messages.UserId), message.UserId);
					FillDateTimeParameter(collection, nameof(Messages.Time), message.Time);
				});
		}
	}
}