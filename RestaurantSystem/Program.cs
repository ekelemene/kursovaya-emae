using ConsoleApp1;
using System;
using System.Linq;

namespace RestaurantManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = new RestaurantSystem();
            try
            {
                system.InitializeData();
            }
            catch (Exception ex)
            {
                PrintError($"Ошибка при загрузке данных: {ex.Message}");
                return;
            }

            Client currentClient = null;
            bool isAdmin = false;

            while (true)
            {
                Console.Clear();
                DisplayHeader(currentClient);

                if (isAdmin)
                    DisplayAdminMenu(system, ref isAdmin, ref currentClient);
                else
                    DisplayClientMenu(system, ref currentClient, ref isAdmin);

                Console.Write("\nВаш выбор: ");
                string choice = Console.ReadLine();
                ProcessChoice(choice, system, ref currentClient, ref isAdmin);
            }
        }

        static void DisplayHeader(Client client)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║       Ресторан «Вкусный уголок»      ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine(client == null
                ? "\nГость, пожалуйста, зарегистрируйтесь."
                : $"\nДобро пожаловать, {client.FullName}!");
        }

        static void DisplayAdminMenu(RestaurantSystem system, ref bool isAdmin, ref Client currentClient)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nРежим администратора:");
            Console.WriteLine("1. Добавить официанта");
            Console.WriteLine("2. Уволить официанта");
            Console.WriteLine("3. Показать популярные блюда");
            Console.WriteLine("4. Показать клиентов по сумме заказа");
            Console.WriteLine("5. Показать средний чек");
            Console.WriteLine("6. Показать общую выручку");
            Console.WriteLine("7. Показать официантов для клиента");
            Console.WriteLine("8. Показать список столов");
            Console.WriteLine("9. Показать список клиентов");
            Console.WriteLine("10. Показать список официантов");
            Console.WriteLine("11. Вернуться в клиентское меню");
            Console.WriteLine("12. Выход");
            Console.ResetColor();
        }

        static void DisplayClientMenu(RestaurantSystem system, ref Client currentClient, ref bool isAdmin)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Зарегистрироваться");
            Console.WriteLine("2. Забронировать столик");
            Console.WriteLine("3. Посмотреть меню");
            Console.WriteLine("4. Сделать заказ");
            Console.WriteLine("5. Посмотреть историю заказов");
            Console.WriteLine("6. Войти как администратор");
            Console.WriteLine("7. Выход");
            Console.ResetColor();
        }

        static void ProcessChoice(string choice, RestaurantSystem system, ref Client currentClient, ref bool isAdmin)
        {
            Console.Clear();
            if (isAdmin)
                ProcessAdminChoice(choice, system, ref isAdmin);
            else
                ProcessClientChoice(choice, system, ref currentClient, ref isAdmin);

            Console.ReadKey();
        }

        static void ProcessAdminChoice(string choice, RestaurantSystem system, ref bool isAdmin)
        {
            switch (choice)
            {
                case "1": // Добавить официанта
                    Console.Write("Введите ФИО официанта: ");
                    string waiterName = Console.ReadLine();
                    Console.Write("Введите ID столиков (через пробел): ");
                    try
                    {
                        var tableIds = Console.ReadLine().Split().Select(int.Parse).ToList();
                        system.AddWaiter(waiterName, tableIds);
                        PrintSuccess("Официант добавлен!");
                    }
                    catch
                    {
                        PrintError("Неверный ввод ID столиков.");
                    }
                    break;

                case "2": // Уволить официанта
                    Console.WriteLine("Список официантов:");
                    foreach (var waiter in system.GetWaiters())
                        Console.WriteLine($"ID: {waiter.Id}, ФИО: {waiter.FullName}");
                    Console.Write("Введите ID официанта: ");
                    if (int.TryParse(Console.ReadLine(), out int waiterId))
                    {
                        system.RemoveWaiter(waiterId);
                        PrintSuccess("Официант уволен!");
                    }
                    else
                    {
                        PrintError("Неверный ID.");
                    }
                    break;

                case "3": // Популярные блюда
                    Console.WriteLine("Топ-3 популярных блюда:");
                    foreach (var dish in system.GetPopularDishes())
                        Console.WriteLine($"{dish.Name} ({dish.Category}) - {dish.Price:F2} руб.");
                    break;

                case "4": // Клиенты по сумме
                    Console.Write("Введите минимальную сумму: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal minAmount))
                    {
                        Console.WriteLine($"Клиенты с заказами от {minAmount:F2} руб.:");
                        foreach (var client in system.GetClientsByOrderAmount(minAmount))
                            Console.WriteLine($"ID: {client.Id}, ФИО: {client.FullName}, Телефон: {client.Phone}");
                    }
                    else
                    {
                        PrintError("Неверная сумма.");
                    }
                    break;

                case "5": // Средний чек
                    Console.Write("Введите дату начала (гггг-мм-дд): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime start))
                    {
                        Console.Write("Введите дату конца (гггг-мм-дд): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime end))
                        {
                            var avgCheck = system.GetAverageCheck(start, end);
                            Console.WriteLine($"Средний чек: {avgCheck:F2} руб.");
                        }
                        else
                        {
                            PrintError("Неверный формат даты конца.");
                        }
                    }
                    else
                    {
                        PrintError("Неверный формат даты начала.");
                    }
                    break;

                case "6": // Общая выручка
                    Console.WriteLine($"Общая выручка: {system.GetTotalRevenue():F2} руб.");
                    break;

                case "7": // Официанты для клиента
                    Console.Write("Введите ID клиента: ");
                    if (int.TryParse(Console.ReadLine(), out int clientId))
                    {
                        Console.WriteLine("Официанты, обслуживавшие клиента:");
                        foreach (var waiter in system.GetWaitersForClient(clientId))
                            Console.WriteLine($"ID: {waiter.Id}, ФИО: {waiter.FullName}");
                    }
                    else
                    {
                        PrintError("Неверный ID.");
                    }
                    break;

                case "8": // Показать список столов
                    Console.WriteLine("Список столов:");
                    foreach (var table in system.GetTables())
                        Console.WriteLine($"ID: {table.Id}, Зал: {table.Hall}, Мест: {table.Seats}, Доступен: {(table.IsAvailable ? "Да" : "Нет")}");
                    break;

                case "9": // Показать список клиентов
                    Console.WriteLine("Список клиентов:");
                    foreach (var client in system.GetClients())
                    {
                        var orders = string.Join(", ", client.OrderIds);
                        var reservation = client.ReservationTableId.HasValue ? $"Столик {client.ReservationTableId}" : "Нет";
                        Console.WriteLine($"ID: {client.Id}, ФИО: {client.FullName}, Телефон: {client.Phone}, Заказы: {(string.IsNullOrEmpty(orders) ? "Нет" : orders)}, Бронь: {reservation}");
                    }
                    break;

                case "10": // Показать список официантов
                    Console.WriteLine("Список официантов:");
                    foreach (var waiter in system.GetWaiters())
                    {
                        var tables = string.Join(", ", waiter.AssignedTableIds);
                        var orders = string.Join(", ", waiter.ServedOrderIds);
                        Console.WriteLine($"ID: {waiter.Id}, ФИО: {waiter.FullName}, Столики: {(string.IsNullOrEmpty(tables) ? "Нет" : tables)}, Заказы: {(string.IsNullOrEmpty(orders) ? "Нет" : orders)}");
                    }
                    break;

                case "11": // Вернуться в клиентское меню
                    isAdmin = false;
                    break;

                case "12": // Выход
                    PrintMessage("Спасибо за посещение!", ConsoleColor.Yellow);
                    Environment.Exit(0);
                    break;

                default:
                    PrintError("Неверный выбор.");
                    break;
            }
        }

        static void ProcessClientChoice(string choice, RestaurantSystem system, ref Client currentClient, ref bool isAdmin)
        {
            switch (choice)
            {
                case "1": // Регистрация
                    if (currentClient != null)
                    {
                        PrintError("Вы уже зарегистрированы!");
                        return;
                    }
                    Console.Write("Введите ФИО: ");
                    string fullName = Console.ReadLine();
                    Console.Write("Введите телефон: ");
                    string phone = Console.ReadLine();
                    try
                    {
                        currentClient = system.RegisterClient(fullName, phone);
                        PrintSuccess($"Регистрация успешна! Ваш ID: {currentClient.Id}");
                    }
                    catch
                    {
                        PrintError("Ошибка при регистрации.");
                    }
                    break;

                case "2": // Бронирование столика
                    if (currentClient == null)
                    {
                        PrintError("Сначала зарегистрируйтесь!");
                        return;
                    }
                    Console.Write("Введите дату и время (гггг-мм-дд чч:мм): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dateTime))
                    {
                        Console.WriteLine("Доступные столики:");
                        var tables = system.GetAvailableTables(dateTime);
                        foreach (var table in tables)
                            Console.WriteLine($"ID: {table.Id}, Зал: {table.Hall}, Мест: {table.Seats}");
                        Console.Write("\nВведите ID столика: ");
                        if (int.TryParse(Console.ReadLine(), out int tableId))
                        {
                            if (system.ReserveTable(currentClient.Id, tableId, dateTime))
                                PrintSuccess("Столик успешно забронирован!");
                            else
                                PrintError("Ошибка: столик недоступен или не существует.");
                        }
                        else
                        {
                            PrintError("Неверный ID столика.");
                        }
                    }
                    else
                    {
                        PrintError("Неверный формат даты.");
                    }
                    break;

                case "3": // Просмотр меню
                    Console.WriteLine("Меню ресторана:");
                    foreach (var dish in system.GetMenu())
                        Console.WriteLine($"ID: {dish.Id}, {dish.Name} ({dish.Category}) - {dish.Price:F2} руб.");
                    break;

                case "4": // Сделать заказ
                    if (currentClient == null)
                    {
                        PrintError("Сначала зарегистрируйтесь!");
                        return;
                    }
                    if (currentClient.ReservationTableId == null)
                    {
                        PrintError("Сначала забронируйте столик!");
                        return;
                    }
                    Console.WriteLine("Меню:");
                    foreach (var dish in system.GetMenu())
                        Console.WriteLine($"ID: {dish.Id}, {dish.Name} ({dish.Category}) - {dish.Price:F2} руб.");
                    Console.Write("\nВведите ID блюд (через пробел): ");
                    try
                    {
                        var dishIds = Console.ReadLine().Split().Select(int.Parse).ToList();
                        if (system.CreateOrder(currentClient.Id, currentClient.ReservationTableId.Value, dishIds))
                            PrintSuccess("Заказ успешно создан!");
                        else
                            PrintError("Ошибка при создании заказа.");
                    }
                    catch
                    {
                        PrintError("Неверный ввод ID блюд.");
                    }
                    break;

                case "5": // История заказов
                    if (currentClient == null)
                    {
                        PrintError("Сначала зарегистрируйтесь!");
                        return;
                    }
                    Console.WriteLine("Ваши заказы:");
                    var orders = system.GetClientOrders(currentClient.Id);
                    if (!orders.Any())
                        Console.WriteLine("Заказов нет.");
                    foreach (var order in orders)
                    {
                        var client = system.GetClients().FirstOrDefault(c => c.Id == order.ClientId);
                        var waiter = system.GetWaiters().FirstOrDefault(w => w.Id == order.WaiterId);
                        var dishNames = string.Join(", ", order.DishIds.Select(id => system.GetMenu().FirstOrDefault(d => d.Id == id)?.Name ?? "Неизвестно"));
                        Console.WriteLine($"Заказ #{order.Id}, Клиент: {client?.FullName ?? "Неизвестно"}, Столик: {order.TableId}, Официант: {waiter?.FullName ?? "Неизвестно"}, Сумма: {order.TotalAmount:F2} руб., Статус: {order.Status}, Блюда: {dishNames}");
                    }
                    break;

                case "6": // Вход как администратор
                    Console.Write("Введите пароль админа (для теста: admin): ");
                    if (Console.ReadLine().Trim() == "admin")
                    {
                        isAdmin = true;
                        PrintSuccess("Вход в режим администратора успешен!");
                    }
                    else
                    {
                        PrintError("Неверный пароль.");
                    }
                    break;

                case "7": // Выход
                    PrintMessage("Спасибо за посещение!", ConsoleColor.Yellow);
                    Environment.Exit(0);
                    break;

                default:
                    PrintError("Неверный выбор.");
                    break;
            }
        }

        static void PrintSuccess(string message)
        {
            PrintMessage(message, ConsoleColor.Green);
        }

        static void PrintError(string message)
        {
            PrintMessage(message, ConsoleColor.Red);
        }

        static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}