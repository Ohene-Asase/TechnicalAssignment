using Microsoft.EntityFrameworkCore;
using TechnicalAssignment.DBContext;
using TechnicalAssignment.Models;

namespace TechnicalAssignment.Repositories
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public int? Quantity { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? OrderNumber { get; set; }
        public decimal TotalCost { get; set; }
        public int UserId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = null!;
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }

    }

    public class CreateOrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = null!;
    }

    public class CreateOrderDetailDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }


    public class OrderRepository
    {
        private readonly InMemoryDbContext _context;

        public OrderRepository(InMemoryDbContext context)
        {
            _context = context;
        }
        public IEnumerable<OrderDto> GetOrders()
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(q=>q.Product)
                .Include(o => o.User)
                .ToList();

            var orderDtos = orders.Select(order => new OrderDto
            {
                Id=order.Id,
                Date = order.Date,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                TotalCost = order.OrderDetails.Sum(q => q.Cost),
                OrderDetails = order.OrderDetails.Select(p =>new OrderDetailDto{
                    Id=p.Id,
                    ProductId=p.ProductId,
                    ProductName=p.Product.Name,
                    Quantity=p.Quantity,
                    Price=p.Price,
                    Cost =p.Cost,
                }).ToList()
            }).ToList();

            return orderDtos;
        }

        public OrderDto GetOrderById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID. ID must be a positive integer value.");
            }

            var order = _context.Orders
                .Include(o => o.OrderDetails).ThenInclude (q => q.Product)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                throw new ArgumentException($"No order found with ID {id}.");
            }

            return new OrderDto
            {
                Id = order.Id,
                Date = order.Date,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                TotalCost = order.OrderDetails.Sum(q => q.Cost),
                OrderDetails = order.OrderDetails.Select(p => new OrderDetailDto
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProductName = p.Product.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Cost = p.Cost,
                }).ToList()
            };
        }

        public async Task<long> AddOrder(CreateOrderDto record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record), "Order record cannot be null.");
            }

            var user = await _context.Users.FindAsync(record.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {record.UserId} not found.");
            }

            if (record.OrderDetails == null || !record.OrderDetails.Any())
            {
                throw new ArgumentException("Product list cannot be null or empty.");
            }
            var lastOrder=await _context.Orders.LastOrDefaultAsync();
            int orderNumber=1;
            if(lastOrder != null) { orderNumber = lastOrder.Id + 1; }
           
            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(orderNumber),
                Date=DateTime.UtcNow,
                UserId = record.UserId,
                User = user,
                OrderDetails = record.OrderDetails.Select(x=>new OrderDetail
                {
                    //Id= x.Id,
                    ProductId=x.ProductId,
                    Price  =x.Price,
                    Quantity =x.Quantity,
                    Cost=x.Price*x.Quantity,
                }).ToList()
            };

            //foreach (var productId in record.Products)
            //{
            //    var product = await _context.Products.FindAsync(productId);
            //    if (product == null)
            //    {
            //        throw new ArgumentException($"Product with ID {productId} not found.");
            //    }
            //    order.Products.Add(product);
            //}

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        private string GenerateOrderNumber(int entryNumber)
        {
            string formattedDate = DateTime.Now.ToString("yyyyMMdd");
            string receiptNumber = $"{formattedDate}/{entryNumber:D5}";
            return receiptNumber;
        }



        public void UpdateOrder(CreateOrderDto updatedOrder)
        {
            if (updatedOrder == null)
            {
                throw new ArgumentNullException(nameof(updatedOrder), "Updated order cannot be null.");
            }

            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (order == null)
            {
                throw new ArgumentException($"Order with ID {updatedOrder.Id} not found.");
            }


            //order.Date = updatedOrder.Date;
            order.UserId = updatedOrder.UserId;
           order.TotalCost=updatedOrder.OrderDetails.Sum(x=>(x.Price*x.Quantity));

            order.OrderDetails.Clear();
            order.OrderDetails = updatedOrder.OrderDetails.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                Cost = x.Price * x.Quantity,
            }).ToList();

            _context.Update(order);
            _context.SaveChanges();
            
            //order.OrderDetails.Clear();
            //_context.SaveChanges()

        }


        public void DeleteOrder(int id)
        {
            var order = _context.Orders
                //.Include(o => o.Products)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                // Handle the case when the order is not found
                return;
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }



    }
}
