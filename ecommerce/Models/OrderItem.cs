using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
	public class OrderItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? OrderId { get; set; }
		[ForeignKey("OrderId")]
		public Order Order { get; set; }
		public int? ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

	}
}
