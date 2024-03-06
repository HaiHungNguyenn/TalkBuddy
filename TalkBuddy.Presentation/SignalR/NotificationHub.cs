using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.SignalR
{
    public class NotificationHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        private readonly IFriendShipService _friendShipService;
        private readonly INotificationService _notificationService;
        private readonly IClientService _clientService;

        public NotificationHub(IFriendShipService friendShipService, INotificationService notificationService, IClientService clientService)
        {
            _friendShipService = friendShipService;
            _notificationService = notificationService;
            _clientService = clientService;
        }

        // public async Task SendNotificationToUser(string userId, string message)
        // {
        //     if (UserConnections.TryGetValue(userId, out var connectionId))
        //     {
        //         await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
        //     }
        // }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            var connectionId = Context.ConnectionId;
            UserConnections.AddOrUpdate(userId, connectionId, (key, oldValue) => connectionId);
           
            Console.WriteLine(connectionId);
            Console.WriteLine("+++++++++++++++++++++++++++=");
            // httpContext.Session.SetString("connectionId",connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Session.GetString(SessionConstants.USER_ID);
            UserConnections.TryRemove(userId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task HandleAccept(Guid friendshipId,Guid senderId, Guid receiverId)
        {
            await _friendShipService.AcceptFriendInvitation(friendshipId);
            var sender = await _clientService.GetClientById(senderId) ?? throw new Exception("Not found user");
            var receiver = await _clientService.GetClientById(receiverId)?? throw new Exception("Not found user");
         
            await _notificationService.CreateNotification(new Notification()
            {
                Message = $"{sender.Name} has just accepted your invitation",
                ClientId = senderId,
            });
            var notifications =await _notificationService.GetNotificationByClient(senderId);
            await SendNotification(senderId.ToString(), notifications.ToList());
        }
        public async Task HandleReject(Guid friendshipId,Guid senderId, Guid receiverId)
        {
            await _friendShipService.RejectFriendInvitation(friendshipId);
            var sender = await _clientService.GetClientById(senderId) ?? throw new Exception("Not found user");
            var receiver = await _clientService.GetClientById(receiverId)?? throw new Exception("Not found user");
         
            await _notificationService.CreateNotification(new Notification()
            {
                Message = $"{sender.Name} has just rejected your invitation",
                ClientId = senderId
            });
            var notifications =await _notificationService.GetNotificationByClient(senderId);
            await SendNotification(senderId.ToString(), notifications.ToList());
        }
        

        public async Task SendNotification(string receiverId, List<Notification> notifications)
        {
            var connectionId = UserConnections[receiverId];
            var x = Clients.Client(connectionId);
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", connectionId);
        }
    }
}
