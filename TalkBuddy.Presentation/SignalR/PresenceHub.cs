using Microsoft.AspNetCore.SignalR;
using TalkBuddy.Common.Constants;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.SignalR
{
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;
        private readonly IFriendShipService _friendShipService;        

        public PresenceHub(PresenceTracker presenceTracker, IFriendShipService friendShipService)
        {
            _presenceTracker = presenceTracker;
            _friendShipService = friendShipService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            var isOnline = await _presenceTracker.UserConnected(userId, Context.ConnectionId);
            var friends = await _friendShipService.GetClientFriends(Guid.Parse(userId));    
            var onlineFirends = new List<Guid>();
            if (isOnline)
            {
                foreach(var friend in friends)
                {
                    if (IsUserOnline(friend.Id.ToString()))
                    {
                        onlineFirends.Add(friend.Id);
                        await Groups.AddToGroupAsync(Context.ConnectionId, friend.Id.ToString());
                        var friendConnectionId = PresenceTracker.GetConnectionsForUser(friend.Id.ToString()).Result.ToList().FirstOrDefault();
                        await Groups.AddToGroupAsync(friendConnectionId, userId.ToString());
                        await Clients.Groups(friend.Id.ToString()).SendAsync("GetOnlineUsers", userId);
                    }
                }              
                await Clients.Groups(userId).SendAsync("UpdateOnlineStatus", userId, true);
                await Clients.Caller.SendAsync("GetOnlineFriends", onlineFirends);
            }            
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            var isOffline = await _presenceTracker.UserDisconnected(userId, Context.ConnectionId);
            if (isOffline)
            {
                await Clients.Groups(userId).SendAsync("UpdateOnlineStatus", userId, false);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public bool IsUserOnline(string userId)
        {
            return _presenceTracker.GetOnlineUsers().Result.Contains(userId);
        }
    }
}
