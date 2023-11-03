namespace TechnicalAssignment.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Order Order { get; set; } = null!;
        public int OrderId { get; set; }
        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set;}

    }
}
