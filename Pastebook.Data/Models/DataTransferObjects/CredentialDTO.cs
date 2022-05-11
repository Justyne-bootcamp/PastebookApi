using System;
namespace Pastebook.Data.Models.DataTransferObjects
{
    public class CredentialDTO
    {
        public Guid UserAccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}
