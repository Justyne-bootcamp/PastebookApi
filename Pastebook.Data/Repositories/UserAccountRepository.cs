using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Pastebook.Data.Repositories
{
    public interface IUserAccountRepository: IBaseRepository<UserAccount> 
    {
        public string CreateUsername(string firstName, string lastName);
        public string CheckUsernameExist(string concattedName);
        public string GetHash(string password, string salt);
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
            return concattedName;
        }

        public string GetHash(string password, string salt)
        {
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(String.Concat(salt, password));

            SHA256Managed sha256 = new SHA256Managed();
            byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);
            string hashedString = Convert.ToBase64String(hashedBytes);

            return hashedString;
        }
    }
}
