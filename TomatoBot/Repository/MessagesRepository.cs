using TomatoBot.Model;

namespace TomatoBot.Repository
{
	public class MessagesRepository : DbRepository
	{
		public void AddMessage(Messages message)
		{
			var query = $"insert into {nameof(Messages)} " +
				$"({nameof(Messages.ConversationId)}, " +
			            $"{nameof(Messages.UserId)}, " +
			            $"{nameof(Messages.WordsCount)}, " +
			            $"{nameof(Messages.Time)}, " +
						$"{nameof(Messages.SmilesCount)}, " +
						$"{nameof(Messages.AttachmentsCount)}, " +
						$"{nameof(Messages.LinksCount)}, " +
						$"{nameof(Messages.MessageLength)}, " +
						$"{nameof(Messages.ReplyToId)}) values " +
				$"(@{nameof(Messages.ConversationId)}, " +
			            $"@{nameof(Messages.UserId)}, " +
			            $"@{nameof(Messages.WordsCount)}, " +
			            $"@{nameof(Messages.Time)}, " +
						$"@{nameof(Messages.SmilesCount)}, " +
						$"@{nameof(Messages.AttachmentsCount)}, " +
						$"@{nameof(Messages.LinksCount)}, " +
						$"@{nameof(Messages.MessageLength)}, " +
						$"@{nameof(Messages.ReplyToId)})";

			ExecuteNonQuery(
				query,
				collection =>
				{
					FillStringParameter(collection, nameof(Messages.ConversationId), message.ConversationId);
					FillIntParameter(collection, nameof(Messages.WordsCount), message.WordsCount);
					FillStringParameter(collection, nameof(Messages.UserId), message.UserId);
					FillDateTimeParameter(collection, nameof(Messages.Time), message.Time);
					FillIntParameter(collection, nameof(Messages.SmilesCount), message.SmilesCount);
					FillIntParameter(collection, nameof(Messages.AttachmentsCount), message.AttachmentsCount);
					FillIntParameter(collection, nameof(Messages.LinksCount), message.LinksCount);
					FillIntParameter(collection, nameof(Messages.MessageLength), message.MessageLength);
					FillIntParameter(collection, nameof(Messages.ReplyToId), message.ReplyToId);
				});
		}
	}
}