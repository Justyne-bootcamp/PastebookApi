using Pastebook.Data.Data;
using Pastebook.Data.Models;
using Pastebook.Data.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastebook.Data.Exceptions;

namespace Pastebook.Data.Repositories
{
    public interface IUserAccountRepository: IBaseRepository<UserAccount> 
    {
        public CredentialDTO FindByEmail(string email);
        public bool FindEmail(string email);
    }
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(PastebookContext context) : base(context)
        {
        }

        public bool FindEmail(string email)
        {
            var doesExist = this.Context.UserAccounts
                .Select(e => e)
                .Where(e => e.Email.Equals(email))
                .FirstOrDefault();
            if(doesExist is object)
            {
                return true;
            }
            else
            {
                throw new InvalidCredentialException($"Invalid Credential. No user with email: {email} found.");
            }
        }
        public CredentialDTO FindByEmail(string email)
        {
            var userAccount = this.Context.UserAccounts
                .Select(e => new CredentialDTO() {
                    Email = e.Email,
                    Password = e.Password,
                    FirstName = e.FirstName,
                    LastName = e.LastName
                })
                .Where(e => e.Email.Equals(email))
                .FirstOrDefault();
            return userAccount;
        }
    }
}
