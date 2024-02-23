using TalkBuddy.Common.Enums;

namespace TalkBuddy.Domain.Dtos;

public class DtoClientForFriend
{
    public Guid id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public FriendShipRequestStatus? RelationStatus { get; set; } = FriendShipRequestStatus.WAITING;
    public string? ProfilePicture { get; set; }
}