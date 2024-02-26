using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities
{
    public class UserConnection : BaseEntity<string>
    {
        public string ChatBoxId { get; set; }
        public string UserName { get; set; }
    }
}
