using System;
namespace Pastebook.Data.Models.DataTransferObjects
{
    public class CredentialDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
