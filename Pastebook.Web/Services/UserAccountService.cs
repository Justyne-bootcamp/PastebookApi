﻿using Pastebook.Data.Models;
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

        public UserAccount Insert(UserAccount userAccount);
    }
    public class UserAccountService: IUserAccountService
    {
        private IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        public async Task<IEnumerable<UserAccount>> FindAll()
        {
            return await _userAccountRepository.FindAll();
        }

        public async Task<UserAccount> FindById(Guid id)
        {
            return await _userAccountRepository.FindByPrimaryKey(id);
        }

        public UserAccount Insert(UserAccount userAccount)
        {
            return _userAccountRepository.Insert(userAccount);
        }
    }
}
