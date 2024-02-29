using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Domain.Dtos
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime SentDate { get; set; }

        public Guid ChatBoxId { get; set; }
        public Guid SenderId { get; set; }

        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
        public virtual ICollection<Media> Medias { get; set; } = new List<Media>();
    }
}
