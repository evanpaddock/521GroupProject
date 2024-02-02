using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronParadise.Models
{
    public class User : IdentityUser
    {
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        //Ability to delete account (soft delete)
        public bool isActiveAccount { get; set; } = true;

	}
}
