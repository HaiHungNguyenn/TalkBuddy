using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Dtos;
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
        private readonly IMapper _mapper;
        public NotificationHub(IFriendShipService friendShipService, INotificationService notificationService, IClientService clientService, IMapper mapper)
        {
            _friendShipService = friendShipService;
            _notificationService = notificationService;
            _clientService = clientService;
            _mapper = mapper;
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
            
            
            var notifications = (await _notificationService.GetNotificationByClient(new Guid(userId))).Include(x => x.Client);
            var dtoNotifications = 
                notifications.Select(x => (new DtoNotification()
                {
                    Message = x.Message,
                    ClientId = x.ClientId,
                    IsRead = x.IsRead,
                    SendAt = x.SendAt,
                    ClientAvatar = x.Client.ProfilePicture
                }));
            await SendNotification(userId, dtoNotifications.OrderByDescending(x => x.SendAt).ToList());
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
                Message = $"{receiver.Name} has just accepted your invitation",
                ClientId = senderId,
                SendAt = DateTime.Now
            });
            // var notifications =(await _notificationService.GetNotificationByClient(senderId)).ProjectTo<DtoNotification>(_mapper.ConfigurationProvider);
            var notifications = (await _notificationService.GetNotificationByClient(senderId)).Include(x => x.Client);
            var dtoNotifications = 
                notifications.Select(x => (new DtoNotification()
                {
                    Message = x.Message,
                    ClientId = x.ClientId,
                    IsRead = x.IsRead,
                    SendAt = x.SendAt,
                    ClientAvatar = receiver.ProfilePicture
                }));
            await SendNotification(senderId.ToString(), dtoNotifications.OrderByDescending(x => x.SendAt).ToList());
        }
        public async Task HandleReject(Guid friendshipId,Guid senderId, Guid receiverId)
        {
            await _friendShipService.RejectFriendInvitation(friendshipId);
            var sender = await _clientService.GetClientById(senderId) ?? throw new Exception("Not found user");
            var receiver = await _clientService.GetClientById(receiverId)?? throw new Exception("Not found user");
         
            await _notificationService.CreateNotification(new Notification()
            {
                Message = $"{receiver.Name} has just rejected your invitation",
                ClientId = senderId,
                SendAt = DateTime.Now

            });
            // var notifications = (await _notificationService.GetNotificationByClient(senderId)).ProjectTo<DtoNotification>(_mapper.ConfigurationProvider);

            var notifications = (await _notificationService.GetNotificationByClient(senderId)).Include(x => x.Client);
            var dtoNotifications = 
            notifications.Select(x => (new DtoNotification()
            {
                Message = x.Message,
                ClientId = x.ClientId,
                IsRead = x.IsRead,
                SendAt = x.SendAt,
                ClientAvatar = receiver.ProfilePicture
            }));
            await SendNotification(senderId.ToString(), dtoNotifications.OrderByDescending(x => x.SendAt).ToList());
        }
        

        public async Task SendNotification(string receiverId, List<DtoNotification> notifications)
        {
            var connectionId = UserConnections[receiverId];
            var x = Clients.Client(connectionId);
            await Clients.Client(connectionId).SendAsync("ReceiveNotification", notifications,connectionId);
        }

        public async Task UpdateNotificationStatus()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext!.Session.GetString(SessionConstants.USER_ID) ?? throw new Exception("Not found user");
            var notifications = await _notificationService.GetNotificationByClient(new Guid(userId));
            await _notificationService.UpdateNotificationStatus(notifications);
        }
        public async Task HandleAddFriend(string friendId)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext!.Session.GetString(SessionConstants.USER_ID) ?? throw new Exception("Not found user");
            var user = await _clientService.GetClientById(new Guid(userId));
            await _friendShipService.CreateFriendship(new Guid(userId), new Guid(friendId));
            await _notificationService.CreateNotification(new Notification()
            {
                Message = $"{user.Name} has just sent you an invitation",
                ClientId = new Guid(friendId),
                SendAt = DateTime.Now
            });
            var notifications = (await _notificationService.GetNotificationByClient(new Guid(friendId))).Include(x => x.Client);
            var dtoNotifications = 
                notifications.Select(x => (new DtoNotification()
                {
                    Message = x.Message,
                    ClientId = x.ClientId,
                    IsRead = x.IsRead,
                    SendAt = x.SendAt,
                    ClientAvatar = x.Client.ProfilePicture
                }));
            await SendNotification(friendId, dtoNotifications.OrderByDescending(x => x.SendAt).ToList());
        }
    }
}
