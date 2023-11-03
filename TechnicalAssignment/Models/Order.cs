namespace TechnicalAssignment.Models
{
    public class Order
    {

        public int Id { get; set; } 
        public string? OrderNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
        public int UserId { get; set; }    
        public User? User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = null!;

    }
}
