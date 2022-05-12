using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Models.DataTransferObjects
{
    public class SearchResultDTO
    {
        public Guid UserAccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhotoPath { get; set; }
        public string Username { get; set; }
    }
}
