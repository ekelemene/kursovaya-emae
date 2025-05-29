using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    internal class RestaurantSystem
    {
        private List<Table> Tables { get; } = new List<Table>();
        private List<Client> Clients { get; set; } = new List<Client>();
        private List<Waiter> Waiters { get; } = new List<Waiter>();
        private List<Dish> Dishes { get; } = new List<Dish>();
        private List<Order> Orders { get; } = new List<Order>();

        // Пути в корне проекта
        private readonly string tablesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "tables.txt");
        private readonly string clientsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "clients.txt");
        private readonly string waitersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "waiters.txt");
        private readonly string dishesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "dishes.txt");
        private readonly string ordersFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "orders.txt");

        public void InitializeData()
        {
            Console.WriteLine("Начало инициализации данных...");
            try
            {
                LoadTables();
                LoadClients();
                LoadWaiters();
                LoadDishes();
                LoadOrders();
                Console.WriteLine("Данные инициализированы!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации: {ex.Message}");
                throw;
            }
        }

        private void LoadTables()
        {
            if (!File.Exists(tablesFile)) CreateDefaultTables();
            Tables.Clear();
            foreach (var line in File.ReadAllLines(tablesFile))
            {
                var parts = line.Split(',');
                Tables.Add(new Table
                {
                    Id = int.Parse(parts[0]),
                    Hall = parts[1],
                    Seats = int.Parse(parts[2]),
                    IsAvailable = bool.Parse(parts[3])
                });
            }
        }

        private void LoadClients()
        {
            if (!File.Exists(clientsFile)) CreateDefaultClients();
            Clients.Clear();
            foreach (var line in File.ReadAllLines(clientsFile))
            {
                var parts = line.Split(',');
                var client = new Client
                {
                    Id = int.Parse(parts[0]),
                    FullName = parts[1],
                    Phone = parts[2],
                    OrderIds = parts[3].Split(';').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList(),
                    ReservationTableId = string.IsNullOrEmpty(parts[4]) ? null : (int?)int.Parse(parts[4])
                };
                Clients.Add(client);
            }
        }

        private void LoadWaiters()
        {
            if (!File.Exists(waitersFile)) CreateDefaultWaiters();
            Waiters.Clear();
            foreach (var line in File.ReadAllLines(waitersFile))
            {
                var parts = line.Split(',');
                Waiters.Add(new Waiter
                {
                    Id = int.Parse(parts[0]),
                    FullName = parts[1],
                    AssignedTableIds = parts[2].Split(';').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList(),
                    ServedOrderIds = parts[3].Split(';').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList()
                });
            }
        }

        private void LoadDishes()
        {
            if (!File.Exists(dishesFile)) CreateDefaultDishes();
            Dishes.Clear();
            foreach (var line in File.ReadAllLines(dishesFile))
            {
                var parts = line.Split(',');
                Dishes.Add(new Dish
                {
                    Id = int.Parse(parts[0]),
                    Name = parts[1],
                    Category = parts[2],
                    Price = decimal.Parse(parts[3])
                });
            }
        }

        private void LoadOrders()
        {
            if (!File.Exists(ordersFile)) CreateDefaultOrders();
            Orders.Clear();
            foreach (var line in File.ReadAllLines(ordersFile))
            {
                var parts = line.Split(',');
                Orders.Add(new Order
                {
                    Id = int.Parse(parts[0]),
                    ClientId = int.Parse(parts[1]),
                    TableId = int.Parse(parts[2]),
                    WaiterId = int.Parse(parts[3]),
                    DishIds = parts[4].Split(';').Select(int.Parse).ToList(),
                    TotalAmount = decimal.Parse(parts[5]),
                    Status = parts[6],
                    OrderDate = DateTime.Parse(parts[7])
                });
            }
        }

        private void CreateDefaultTables()
        {
            Console.WriteLine($"Создание {tablesFile}...");
            var data = new[]
            {
                "1,Основной,4,true",
                "2,Основной,2,false",
                "3,VIP,6,true",
                "4,VIP,4,true"
            };
            File.WriteAllLines(tablesFile, data);
            Console.WriteLine($"{tablesFile} создан!");
        }

        private void CreateDefaultClients()
        {
            Console.WriteLine($"Создание {clientsFile}...");
            var data = new[]
            {
                "1,Иванов Иван,1234567890,1;4,",
                "2,Петров Петр,0987654321,2,3",
                "3,Сидорова Анна,1122334455,3,"
            };
            File.WriteAllLines(clientsFile, data);
            Console.WriteLine($"{clientsFile} создан!");
        }

        private void CreateDefaultWaiters()
        {
            Console.WriteLine($"Создание {waitersFile}...");
            var data = new[]
            {
                "1,Сидоров Алексей,1;2,1;4",
                "2,Козлова Мария,3;4,2;3"
            };
            File.WriteAllLines(waitersFile, data);
            Console.WriteLine($"{waitersFile} создан!");
        }

        private void CreateDefaultDishes()
        {
            Console.WriteLine($"Создание {dishesFile}...");
            var data = new[]
            {
                    "1,Паста Карбонара,Основное,500",
                    "2,Цезарь,Закуски,300",
                    "3,Тирамису,Десерты,400",
                    "4,Стейк,Основное,800",
                    "5,Брускетта,Закуски,250",
                    "6,Чизкейк,Десерты,350",
                    "7,Суп Том Ям,Супы,450",
                    "8,Ризотто,Основное,600",
                    "9,Креветки,Закуски,500",
                    "10,Мороженое,Десерты,200"
                };
            File.WriteAllLines(dishesFile, data);
            Console.WriteLine($"{dishesFile} создан!");
        }

        private void CreateDefaultOrders()
        {
            Console.WriteLine($"Создание {ordersFile}...");
            var data = new[]
            {
                "1,1,1,1,1;2;3,1200,Оплачен,2025-05-17 12:00",
                "2,2,3,2,1;4;6,1650,Оплачен,2025-05-18 12:00",
                "3,3,4,2,2;7;10,950,Оплачен,2025-05-16 12:00",
                "4,1,1,1,1;5;9,1250,Оплачен,2025-05-15 12:00"
            };
            File.WriteAllLines(ordersFile, data);
            Console.WriteLine($"{ordersFile} создан!");
        }

        private void SaveTables()
        {
            var lines = Tables.Select(t => $"{t.Id},{t.Hall},{t.Seats},{t.IsAvailable}");
            File.WriteAllLines(tablesFile, lines);
        }

        private void SaveClients()
        {
            var lines = Clients.Select(c => $"{c.Id},{c.FullName},{c.Phone},{string.Join(";", c.OrderIds ?? new List<int>())},{(c.ReservationTableId.HasValue ? c.ReservationTableId.ToString() : "")}");
            File.WriteAllLines(clientsFile, lines);
        }

        private void SaveWaiters()
        {
            var lines = Waiters.Select(w => $"{w.Id},{w.FullName},{string.Join(";", w.AssignedTableIds ?? new List<int>())},{string.Join(";", w.ServedOrderIds ?? new List<int>())}");
            File.WriteAllLines(waitersFile, lines);
        }

        private void SaveDishes()
        {
            var lines = Dishes.Select(d => $"{d.Id},{d.Name},{d.Category},{d.Price}");
            File.WriteAllLines(dishesFile, lines);
        }

        private void SaveOrders()
        {
            var lines = Orders.Select(o => $"{o.Id},{o.ClientId},{o.TableId},{o.WaiterId},{string.Join(";", o.DishIds)},{o.TotalAmount},{o.Status},{o.OrderDate:yyyy-MM-dd HH:mm}");
            File.WriteAllLines(ordersFile, lines);
        }

        public Client RegisterClient(string fullName, string phone)
        {
            int newId = Clients.Any() ? Clients.Max(c => c.Id) + 1 : 1;
            var client = new Client { Id = newId, FullName = fullName, Phone = phone };
            Clients.Add(client);
            SaveClients();
            return client;
        }

        public bool ReserveTable(int clientId, int tableId, DateTime dateTime)
        {
            var table = Tables.FirstOrDefault(t => t.Id == tableId && t.IsAvailable);
            if (table == null) return false;
            table.IsAvailable = false;
            var client = Clients.FirstOrDefault(c => c.Id == clientId);
            if (client != null) client.ReservationTableId = tableId;
            SaveTables();
            SaveClients();
            return true;
        }

        public bool CreateOrder(int clientId, int tableId, List<int> dishIds)
        {
            var table = Tables.FirstOrDefault(t => t.Id == tableId);
            var client = Clients.FirstOrDefault(c => c.Id == clientId);
            if (table == null || client == null || table.IsAvailable) return false;

            var dishes = Dishes.Where(d => dishIds.Contains(d.Id)).ToList();
            if (!dishes.Any()) return false;

            var waiter = Waiters.FirstOrDefault(w => w.AssignedTableIds.Contains(tableId));
            if (waiter == null) return false;

            int newId = Orders.Any() ? Orders.Max(o => o.Id) + 1 : 1;
            var order = new Order
            {
                Id = newId,
                ClientId = clientId,
                TableId = tableId,
                WaiterId = waiter.Id,
                DishIds = dishIds,
                TotalAmount = dishes.Sum(d => d.Price),
                Status = "Принят",
                OrderDate = DateTime.Now
            };
            Orders.Add(order);
            client.OrderIds.Add(newId);
            waiter.ServedOrderIds.Add(newId);
            SaveOrders();
            SaveClients();
            SaveWaiters();
            return true;
        }

        public List<Order> GetClientOrders(int clientId)
        {
            return Orders.Where(o => o.ClientId == clientId).ToList();
        }

        public List<Table> GetAvailableTables(DateTime dateTime)
        {
            return Tables.Where(t => t.IsAvailable).ToList();
        }

        public List<Dish> GetMenu()
        {
            return Dishes.ToList();
        }

        public List<Dish> GetPopularDishes()
        {
            var dishCounts = Orders.SelectMany(o => o.DishIds)
                .GroupBy(d => d)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => Dishes.First(d => d.Id == g.Key));
            return dishCounts.ToList();
        }

        public List<Client> GetClientsByOrderAmount(decimal minAmount)
        {
            var clientIds = Orders.Where(o => o.TotalAmount >= minAmount)
                .Select(o => o.ClientId)
                .Distinct();
            return Clients.Where(c => clientIds.Contains(c.Id)).ToList();
        }

        public decimal GetAverageCheck(DateTime start, DateTime end)
        {
            var orders = Orders.Where(o => o.OrderDate >= start && o.OrderDate <= end && o.Status == "Оплачен");
            return orders.Any() ? orders.Average(o => o.TotalAmount) : 0;
        }

        public decimal GetTotalRevenue()
        {
            return Orders.Where(o => o.Status == "Оплачен").Sum(o => o.TotalAmount);
        }

        public List<Waiter> GetWaitersForClient(int clientId)
        {
            var waiterIds = Orders.Where(o => o.ClientId == clientId)
                .Select(o => o.WaiterId)
                .Distinct();
            return Waiters.Where(w => waiterIds.Contains(w.Id)).ToList();
        }

        public void AddWaiter(string fullName, List<int> assignedTableIds)
        {
            int newId = Waiters.Any() ? Waiters.Max(w => w.Id) + 1 : 1;
            Waiters.Add(new Waiter
            {
                Id = newId,
                FullName = fullName,
                AssignedTableIds = assignedTableIds ?? new List<int>()
            });
            SaveWaiters();
        }

        public void RemoveWaiter(int waiterId)
        {
            var waiter = Waiters.FirstOrDefault(w => w.Id == waiterId);
            if (waiter != null)
            {
                Waiters.Remove(waiter);
                var orders = Orders.Where(o => o.WaiterId == waiterId).ToList();
                foreach (var order in orders)
                {
                    order.Status = "Отменён";
                }
                SaveWaiters();
                SaveOrders();
            }
        }

        public List<Waiter> GetWaiters()
        {
            return Waiters.ToList();
        }

        public List<Table> GetTables()
        {
            return Tables.ToList();
        }

        public List<Client> GetClients()
        {
            return Clients.ToList();
        }
    }
}