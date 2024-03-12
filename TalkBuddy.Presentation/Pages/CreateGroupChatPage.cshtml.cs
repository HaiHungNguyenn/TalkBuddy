using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<string> SelectedValues { get; set; } = new List<string>();
        private readonly IFriendShipService _friendShipService;
        private readonly IChatBoxService _chatBoxService;
        private readonly IClientService _clientService;

        public CreateGroupChatPageModel(IFriendShipService friendShipService, IChatBoxService chatBoxService, IClientService clientService)
        {
            _friendShipService = friendShipService;
            _chatBoxService = chatBoxService;
            _clientService = clientService;
        }
        public async Task<IActionResult> OnGetAsync(string search = "")
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
            //SelectedValues = TempData["SelectedValues"] as List<string>;
            //TempData.Keep("SelectedValues");
            // Return the same page with the updated FriendsList
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string groupName)
        {
            var selectedValues = Request.Form["selectedFriends"].ToList();
            TempData["SelectedValues"] = selectedValues;
            var userId = HttpContext.Session.GetString(SessionConstants.USER_ID);
            var client = await _clientService.GetClientById(Guid.Parse(userId));
            var chatbox = new ChatBox
            {
                ChatBoxName = groupName ?? "",
                CreatedDate = DateTime.Now,
                Type = ChatBoxType.Group,
                GroupCreatorId = new Guid(userId)
            };

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
            foreach (var friendId in selectedValues)
            {
                var friend = await _clientService.GetClientById(new Guid(friendId));
                chatbox.ClientChatBoxes.Add(new ClientChatBox
                {
                    ClientId = new Guid(friendId),
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
