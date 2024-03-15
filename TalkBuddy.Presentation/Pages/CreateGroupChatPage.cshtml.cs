using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TalkBuddy.Common.Constants;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Domain.Enums;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Presentation.Pages
{
    public class CreateGroupChatPageModel : PageModel
    {
        [BindProperty]
        public IEnumerable<Client> FriendsList { get; set; } = new List<Client>();
        public List<string> SelectedValuesString { get; set; } = new List<string>();
        [BindProperty]
        public IEnumerable<Client> SelectedValues { get; set; } = new List<Client>();
        private readonly IFriendShipService _friendShipService;
        private readonly IChatBoxService _chatBoxService;
        private readonly IClientService _clientService;

        public CreateGroupChatPageModel(IFriendShipService friendShipService, IChatBoxService chatBoxService, IClientService clientService)
        {
            _friendShipService = friendShipService;
            _chatBoxService = chatBoxService;
            _clientService = clientService;
        }
        public async Task<IActionResult> OnGetAsync(string search = "", string selectedFriendsHidden = "")
        {
            var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);

            if (string.IsNullOrEmpty(search))
            {
                FriendsList = await _friendShipService.GetClientFriends(new Guid(userId));
            }
            else
            {
                FriendsList = await _friendShipService.GetClientFriendsSearchByName(new Guid(userId), search);
            }
            if (!string.IsNullOrEmpty(selectedFriendsHidden))
            {
                SelectedValuesString = JsonConvert.DeserializeObject<List<string>>(selectedFriendsHidden);
                foreach (var friendId in SelectedValuesString)
                {
                    if (!string.IsNullOrEmpty(friendId))
                    {

                        var friend = await _clientService.GetClientById(new Guid(friendId));

                        SelectedValues = SelectedValues.Append(friend);
                    }

                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string groupName, string selectedFriendsHidden = "")
        {
            if (!string.IsNullOrEmpty(selectedFriendsHidden))
            {
                SelectedValuesString = JsonConvert.DeserializeObject<List<string>>(selectedFriendsHidden);
                foreach (var friendId in SelectedValuesString)
                {
                    if (!string.IsNullOrEmpty(friendId))
                    {
                        var friend = await _clientService.GetClientById(new Guid(friendId));
                        SelectedValues = SelectedValues.Append(friend);
                    }

                }
            }
            var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);
            var client = await _clientService.GetClientById(Guid.Parse(userId));
            var chatbox = new ChatBox
            {
                ChatBoxName = groupName ?? "",
                CreatedDate = DateTime.Now,
                Type = ChatBoxType.Group,
                GroupCreatorId = new Guid(userId)
            };
            chatbox.Messages.Add(new Message
            {
                Content = $"{client.Name} created the group",
                SentDate = DateTime.Now,
                SenderId = new Guid(userId),
                MessageType = MessageTypes.Notification
            });
            chatbox.ClientChatBoxes.Add(new ClientChatBox
            {
                ClientId = new Guid(userId),
                ChatBox = chatbox,
                IsBlocked = false,
                IsLeft = false,
                IsNotificationOn = true,
                IsModerator = true,
                NickName = client.Name
            });
            foreach (var friend in SelectedValues)
            {

                chatbox.ClientChatBoxes.Add(new ClientChatBox
                {
                    ClientId = friend.Id,
                    ChatBox = chatbox,
                    IsBlocked = false,
                    IsLeft = false,
                    IsNotificationOn = true,
                    IsModerator = false,
                    NickName = friend.Name
                });
            }
            await _chatBoxService.CreateNewChatBox(chatbox);

            return RedirectToPage("/ChatPage");
        }

    }
}
