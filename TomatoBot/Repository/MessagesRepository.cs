using System;
using System.Data.SqlClient;
using System.Linq;
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

		public MessageStatistics[] GetTotalStatistics(string conversationId)
		{
			return ExecuteReader(
				StatisticsQuery(),
				MapReaderToStatistics,
				parametes => FillStringParameter(parametes, nameof(Messages.ConversationId), conversationId)).ToArray();
		}

		public MessageStatistics[] GetDailyStatistics(string conversationId)
		{
			return ExecuteReader(
				StatisticsQuery($"and {nameof(Messages.Time)} > @{nameof(Messages.Time)}"),
				MapReaderToStatistics,
				parametes =>
				{
					FillStringParameter(parametes, nameof(Messages.ConversationId), conversationId);
					FillDateTimeParameter(parametes, nameof(Messages.Time), DateTime.UtcNow.Date);
				}).ToArray();
		}

		private string StatisticsQuery(string additionalFiltering = "")
		{
			return $@"
					Select 
						Count(*) as {nameof(MessageStatistics.MessagesCount)},
						Sum({nameof(MessageStatistics.WordsCount)}) as {nameof(MessageStatistics.WordsCount)}, 
						Sum({nameof(MessageStatistics.SmilesCount)}) as {nameof(MessageStatistics.SmilesCount)}, 
						Sum({nameof(MessageStatistics.AttachmentsCount)}) + Sum(LinksCount) as {nameof(MessageStatistics.AttachmentsCount)},
						Sum(MessageLength) / NULLIF(CAST(Sum({nameof(MessageStatistics.WordsCount)}) as FLOAT), 0) as {nameof(MessageStatistics.AverageWordsLength)},
						[{nameof(Users)}].{nameof(MessageStatistics.FirstName)}, 
						[{nameof(Users)}].{nameof(MessageStatistics.Nickname)} from {nameof(Messages)}
					join {nameof(Users)}
					on [{nameof(Users)}].{nameof(Users.UserId)} = {nameof(Messages)}.{nameof(Users.UserId)} 
						and [{nameof(Users)}].{nameof(Users.ConversationId)} = {nameof(Messages)}.{nameof(Messages.ConversationId)}
					where [{nameof(Messages)}].{nameof(Messages.ConversationId)} = @{nameof(Messages.ConversationId)} {additionalFiltering}
						group by 
							[{nameof(Users)}].{nameof(Users.Id)}, 
							[{nameof(Messages)}].{nameof(Messages.ConversationId)}, 
							[{nameof(Users)}].{nameof(Users.UserId)}, 
							[{nameof(Users)}].{nameof(Users.FirstName)}, 
							[{nameof(Users)}].{nameof(Users.Nickname)}
					order by {nameof(MessageStatistics.MessagesCount)} desc";
		}

		private static MessageStatistics MapReaderToStatistics(SqlDataReader reader) => 
			new MessageStatistics(
				reader.GetInt32(reader.GetOrdinal(nameof(MessageStatistics.MessagesCount))),
				reader.GetInt32(reader.GetOrdinal(nameof(MessageStatistics.WordsCount))),
				reader.GetInt32(reader.GetOrdinal(nameof(MessageStatistics.SmilesCount))),
				reader.GetInt32(reader.GetOrdinal(nameof(MessageStatistics.AttachmentsCount))),
				reader.IsDBNull(reader.GetOrdinal(nameof(MessageStatistics.AverageWordsLength))) ? null : (double?)reader.GetDouble(reader.GetOrdinal(nameof(MessageStatistics.AverageWordsLength))),
				reader.GetString(reader.GetOrdinal(nameof(MessageStatistics.FirstName))),
				reader.GetString(reader.GetOrdinal(nameof(MessageStatistics.Nickname))));
	}
}