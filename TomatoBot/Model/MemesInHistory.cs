using System;
using System.Runtime.Serialization;

namespace TomatoBot.Model
{
    [DataContract]
    public sealed class MemesInHistory
    {
        public MemesInHistory(string conversationId, string memesId, DateTime recieveTimeUtc, string sendUser)
        {
            MemesId = memesId;
            RecieveTimeUtc = recieveTimeUtc;
            SendUser = sendUser;
            ConversationId = conversationId;
        }

        [DataMember]
        public string MemesId { get; set; }

        [DataMember]
        public DateTime RecieveTimeUtc { get; set; }

        [DataMember]
        public string SendUser { get; set; }
        
        [DataMember]
        public string ConversationId { get; set; }
    }
}