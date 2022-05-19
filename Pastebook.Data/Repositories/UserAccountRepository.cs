using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Pastebook.Data.Exceptions;
using Pastebook.Data.Models.DataTransferObjects;

namespace Pastebook.Data.Repositories
{
    public interface IUserAccountRepository: IBaseRepository<UserAccount> 
    {
        public string CreateUsername(string firstName, string lastName);
        public string CheckUsernameExist(string concattedName);
        public string GetHash(string password, string salt);
        public CredentialDTO FindByEmail(string email);
        public bool FindEmail(string email);
        public IEnumerable<UserAccount> FindByName(string searchName);
        public UserAccount FindByUsername(string username);
    }
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(PastebookContext context) : base(context)
        {
        }

        public string CreateUsername(string firstName, string lastName)
        {
            string concattedName = firstName.Trim() + lastName.Trim();
            string username = this.CheckUsernameExist(concattedName);
            return username;
        }

        public string CheckUsernameExist(string concattedName)
        {
            for (int disambiguator = 1; disambiguator <= this.Context.Set<UserAccount>().Count(); disambiguator++)
            {

                var isExisting = this.Context.Set<UserAccount>()
                    .Where(x => x.Username.Equals(concattedName + disambiguator));

                if (!isExisting.Any())
                {
                    return concattedName+disambiguator;
                }
            }
            return concattedName+1;
        }

        public string GetHash(string password, string salt)
        {
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(String.Concat(salt, password));

            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);
            string hashedString = Convert.ToBase64String(hashedBytes);

            return hashedString;
        }

        public bool FindEmail(string email)
        {
            var doesExist = this.Context.UserAccounts
                .Select(e => e)
                .Where(e => e.Email.Equals(email))
                .FirstOrDefault();
            if (doesExist is object)
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
                .Select(e => new CredentialDTO()
                {
                    UserAccountId = e.UserAccountId,
                    Email = e.Email,
                    Password = e.Password,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Username = e.Username,
                })
                .Where(e => e.Email.Equals(email))
                .FirstOrDefault();
            if(userAccount is object)
            {
                return userAccount;
            }
            throw new InvalidCredentialException($"Invalid Credential. No user with email: {email} found.");
        }

        public IEnumerable<UserAccount> FindByName(string searchName)
        {
            var usersWithSearchName = this.Context.UserAccounts
                .Where(u => u.FirstName.Contains(searchName) || u.LastName.Contains(searchName))
                .ToList();

            return usersWithSearchName;
        }

        public UserAccount FindByUsername(string username)
        {
            var userAccount = this.Context.UserAccounts
                .Where(u => u.Username.Equals(username))
                .FirstOrDefault();
            if(userAccount != null)
            {
                return userAccount;
            }
            throw new Exception($"No user account with username: {username} found.");
        }
    }
}
