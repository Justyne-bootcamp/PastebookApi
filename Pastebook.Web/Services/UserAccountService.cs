using Pastebook.Data.Models;
using Pastebook.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pastebook.Web.Services
{
    public interface IUserAccountService
    {
        public Task<IEnumerable<UserAccount>> FindAll();
        public Task<UserAccount> FindById(Guid id);
        public string CreateUsername(string firstName, string lastName);
        public string CheckUsernameExist(string concattedName);
        public string GetHashPassword(string password, string salt);
        public UserAccount Insert(UserAccount userAccount);


    }
    public class UserAccountService: IUserAccountService
    {
        private IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public string CheckUsernameExist(string concattedName)
        {
            return _userAccountRepository.CheckUsernameExist(concattedName);
        }

        public string CreateUsername(string firstName, string lastName)
        {
            return _userAccountRepository.CreateUsername(firstName, lastName);
        }

        public async Task<IEnumerable<UserAccount>> FindAll()
        {
            return await _userAccountRepository.FindAll();
        }

        public async Task<UserAccount> FindById(Guid id)
        {
            return await _userAccountRepository.FindByPrimaryKey(id);
        }

        public string GetHashPassword(string password, string salt)
        {
            return _userAccountRepository.GetHash(password, salt);
        }

        public UserAccount Insert(UserAccount userAccount)
        {
            return _userAccountRepository.Insert(userAccount);
        }
    }
}
