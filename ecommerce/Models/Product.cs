using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int Id { get; set; }
		[MaxLength(255)]
		public string Name { get; set; }
		[MaxLength(255)]
		public string Description { get; set; }
		public decimal Price { get; set; }
		[MaxLength(1000)]
		public string Image { get; set; }
		public decimal DiscountValue { get; set; }
		public int Stock { get; set; }
		[NotMapped]
		public IFormFile? ImageFile { get; set; }

		public int? CategoryId { get; set; }
		[ForeignKey("CategoryId")]

		public Category Category { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }

	}
}
