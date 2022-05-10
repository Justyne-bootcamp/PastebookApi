using Pastebook.Data.Models;
using Pastebook.Web.Data;
using System.Threading.Tasks;

namespace Pastebook.Web.Repositories
{
    public interface IUserAccountRepository : IBaseRepository<UserAccount>
    {
        //public Task<UserAccount> EmailDuplicateChecker(string email);
    }
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(PastebookContext context) : base(context)
        {
        }

        //public Task<UserAccount> EmailDuplicateChecker(string email)
        //{
        //    var emailCheck = this.Context.FindAsync<UserAccount>(email);
        //}
    }
}
