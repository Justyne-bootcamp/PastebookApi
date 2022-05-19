using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class UpdateRegistrationDTO
    {
        public Guid UserAccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public char Gender { get; set; }
        public string MobileNumber { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
    }
}
