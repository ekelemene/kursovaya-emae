using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TableId { get; set; }
        public int WaiterId { get; set; }
        public List<int> DishIds { get; set; } = new List<int>();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
