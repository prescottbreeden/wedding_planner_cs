using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace EventPlanner.Models
{

    public class User

    {
        [Key]
        public int UserId { get;set; }
        [Required]
        public string FirstName { get;set; }
        [Required]
        public string LastName { get;set; }
        [EmailAddress]
        [Required]
        public string Email { get;set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password { get;set; }
        // Will not be mapped to your users table!
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get;set; }

    }

    public class LoginUser
    {
        [Key]
        [Required]
        [EmailAddress (ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)] public string Password { get; set; }
    }

    public class Wedding
    {
        public int WeddingId { get; set; }
        public string WedderOneFirstName { get; set; } 
        public string WedderOneLastName { get; set; } 
        public string WedderTwoFirstName { get; set; } 
        public string WedderTwoLastName { get; set; } 
        public DateTime Date { get; set; }
        public string PlaceName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
        public List<Association> Wedders { get; set; }
    }

        public class Guest
    {
        public int GuestId { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public List<Association> Visitors { get; set; }
    }

    public class Association
    {
        public int AssociationId { get; set; }
        public int WeddingId { get; set; } 
        public int GuestId { get; set; } 
        public Wedding Wedding { get; set; } 
        public Guest Guest { get; set; } 
    }

    public class EventPlannerContext : DbContext
    {
        public EventPlannerContext(DbContextOptions<EventPlannerContext> options) : base(options) { }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }
        public DbSet<Association> Associations { get; set; }
    }
}
