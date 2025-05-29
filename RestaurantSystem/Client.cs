using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public List<int> OrderIds { get; set; } = new List<int>();
        public int? ReservationTableId { get; set; }
    }
}
