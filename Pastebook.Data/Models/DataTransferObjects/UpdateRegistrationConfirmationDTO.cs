using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class UpdateRegistrationConfirmationDTO
    {
        public Guid SessionId { get; set; }
        public string Password { get; set; }
    }
}
