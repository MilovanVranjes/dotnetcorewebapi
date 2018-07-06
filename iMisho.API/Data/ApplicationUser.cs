using Microsoft.AspNetCore.Identity;

namespace iMisho.API.Data
{
    public class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Gender { get; set; }
    }
}
