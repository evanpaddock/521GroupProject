using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronParadise.Models
{
    public class UserDetails
    {

        public User? User { get; set; }

        public string? RoleName { get; set; }

    }
}
