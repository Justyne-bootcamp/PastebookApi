using Microsoft.AspNetCore.Mvc;
using Pastebook.Web.Data;

namespace Pastebook.Web.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public BaseController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
