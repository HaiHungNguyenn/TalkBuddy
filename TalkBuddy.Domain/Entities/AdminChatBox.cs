using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkBuddy.Domain.Entities.BaseEntities;

namespace TalkBuddy.Domain.Entities
{
    public class AdminChatBox : BaseEntity<Guid>
    {
        public Guid AdminId { get; set; }
        public Guid ChatBoxId { get; set; }

        public Client Admin { get; set; } = null!;
        public ChatBox ChatBox { get; set; } = null!;
    }
}
