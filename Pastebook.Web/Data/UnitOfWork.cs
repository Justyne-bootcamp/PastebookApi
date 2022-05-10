using Pastebook.Web.Repositories;
using System;
using System.Threading.Tasks;

namespace Pastebook.Web.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        public IUserAccountRepository UserAccountRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private PastebookContext context;

        public IUserAccountRepository UserAccountRepository { get; private set; }

        public UnitOfWork(PastebookContext context)
        {
            this.context = context;
            this.UserAccountRepository = new UserAccountRepository(context);
        }

        public async Task CommitAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
