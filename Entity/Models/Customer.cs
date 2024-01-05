using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class Customer : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [StringLength(30)]
        public string District { get; set; }
        [StringLength(30)]
        public string street { get; set; }
        [StringLength(30)]
        public string city { get; set; }
        [StringLength(30)]
        public string state { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime LastModifiedDate { get; set; }
        [NotMapped]
        public string Role { get; set; } = string.Empty;
    }
}
