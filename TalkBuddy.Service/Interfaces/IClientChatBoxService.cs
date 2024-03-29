﻿using TalkBuddy.Domain.Entities;

namespace TalkBuddy.Service.Interfaces
{
    public interface IClientChatBoxService
    {
        Task<IList<ClientChatBox>> GetClientChatBoxes();
        Task<IList<ClientChatBox>> GetClientChatBoxes(Guid clientId);
        Task<IList<ClientChatBox>> GetClientChatBoxesIncludeNotEmptyMessages(Guid clientId);
        Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId);
        Task<IList<ClientChatBox>> GetClientOfChatBoxes(Guid chatBoxId, Guid userId);
        Task<IList<Client>> GetFriendsNotInChatBoxes(Guid chatBoxId, Guid userId);
        Task<IList<ClientChatBox>> GetClientOfChatBoxesOfAUserBySearchName(string searchName, Guid userId);
        Task RemoveClientFromChatBox(Guid chatBoxId, Guid clientId);
        Task AddClientToGroup(ClientChatBox clientChatBox);
       
        Task Update(ClientChatBox clientChatBox);
        Task<string> GetChatBoxNameOfTwoPersonType(Guid chatBoxId, Guid userId);
    }
}
