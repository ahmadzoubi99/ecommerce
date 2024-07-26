using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class ContactUs
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		[MaxLength(255)]
		public string Email { get; set; }
		[MaxLength(255)]
		public string Subject { get; set; }
		public string Message { get; set; }
		public int? UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }

	}
}
