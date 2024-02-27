using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Domain.Entities;
using TalkBuddy.Service.Interfaces;

namespace TalkBuddy.Service.Implementations
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserConnectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddConnection(UserConnection userConnection)
        {
            await _unitOfWork.UserConnectionRepository.AddAsync(userConnection);
        }

        public async Task<UserConnection> GetConnection(string connectionId, string userName)
        {
            //return await _unitOfWork.UserConnectionRepository.FindAsync(x=>x.Id == connectionId && x.UserName == userName);
        }

        public Task<UserConnection> GetConnection(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveConnection(UserConnection userConnection)
        {
            throw new NotImplementedException();
        }
    }
}
