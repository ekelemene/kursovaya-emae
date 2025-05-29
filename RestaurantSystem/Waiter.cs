using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Waiter
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public List<int> AssignedTableIds { get; set; } = new List<int>();
        public List<int> ServedOrderIds { get; set; } = new List<int>();
    }
}
