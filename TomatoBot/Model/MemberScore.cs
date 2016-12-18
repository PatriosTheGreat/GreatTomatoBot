using System.Runtime.Serialization;

namespace TomatoBot.Model
{
    [DataContract]
    public sealed class MemberScore
    {
        [DataMember]
        public string UserFirstName { get; set; }

        [DataMember]
        public string UserNickname { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string ConversationId { get; set; }

        [DataMember]
        public int Score { get; set; }

        public string PersonalScore()
        {
            var userName = "неизвестный";
            if (!string.IsNullOrEmpty(UserNickname))
            {
                userName = UserNickname;
            }
            else if (!string.IsNullOrEmpty(UserFirstName))
            {
                userName = UserFirstName;
            }

            return $"{userName} {Score}";
        }
    }
}