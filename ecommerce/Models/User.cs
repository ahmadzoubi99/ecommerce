using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[MaxLength(100)]
		public string Username { get; set; }
		[MaxLength(200)]
		public string PasswordHash { get; set; }
		[MaxLength(100)]
		public string Email { get; set; }
		[MaxLength(100)]
		public string FullName { get; set; }
		[MaxLength(255)]
		public string Location { get; set; }
		[MaxLength(50)]
		public string phoneNumber { get; set; }
		[MaxLength(1000)]
		public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public int? RoleId { get; set; }
		[ForeignKey("RoleId")]
		public Role Role { get; set; }
		public DateTime? Birthday { get; set; }
		public ICollection<Order> Orders { get; set; }
		public ICollection<Testimonial> Testimonials { get; set; }


	}
}
