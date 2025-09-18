using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VideoGameApi.Domain.Enums;

namespace VideoGameApi.Domain
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid UserId { get; set; } 
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; } = false;

        [Required]
        [MaxLength(20)]
        public UserRole Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation Property
        public ICollection<UserVideoGame> UserVideoGames { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
