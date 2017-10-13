namespace Shop
{
    using System;
    using System.Linq;
    using Models;

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ShopDbContext())
            {
                PrepareDatabase(db);
                SaveSalesmen(db);
                SaveItems(db);
                ProcessCommands(db);
                //PrintSalesmenWithCustomerCount(db);
                //PrintCustomerOrdersAndReviewsCount(db);
                //PrintOrderItemAndReviewsCountByCustomerId(db);
                //PrintCustomerDataById(db);
                PrintOrdersCountByCustomerId(db);
            }
        }

        private static void PrepareDatabase(ShopDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        private static void SaveSalesmen(ShopDbContext db)
        {
            string[] names = Console.ReadLine().Split(';');

            foreach (var name in names)
            {
                Salesman man = new Salesman { Name = name };
                db.Salesmen.Add(man);
            }

            db.SaveChanges();
        }

        private static void SaveItems(ShopDbContext db)
        {
            while (true)
            {
                string[] line = Console.ReadLine().Split(';');

                if (line[0] == "END")
                {
                    break;
                }

                Item item = new Item { Name = line[0], Price = decimal.Parse(line[1]) };
                db.Items.Add(item);

                db.SaveChanges();
            }
        }

        private static void ProcessCommands(ShopDbContext db)
        {
            while (true)
            {
                string[] line = Console.ReadLine().Split('-');

                if (line[0] == "END")
                {
                    break;
                }

                string command = line[0];
                string arguments = line[1];

                switch (command)
                {
                    case "register": RegisterCustomer(db, arguments); break;
                    case "order": MakeOrder(db, arguments); break;
                    case "review": LeaveReview(db, arguments); break;
                    default:
                        break;
                }
            }
        }

        private static void RegisterCustomer(ShopDbContext db, string arguments)
        {
            string[] details = arguments.Split(';');
            Customer customer = new Customer { Name = details[0], SalesmanId = int.Parse(details[1]) };
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        private static void LeaveReview(ShopDbContext db, string arguments)
        {
            string[] data = arguments.Split(';');
            int customerId = int.Parse(data[0]);
            int itemId = int.Parse(data[1]);

            Review review = new Review { CustomerId = customerId, ItemId = itemId };

            db.Reviews.Add(review);
            db.SaveChanges();
        }

        private static void MakeOrder(ShopDbContext db, string arguments)
        {
            int[] commandArgs = arguments.Split(';').Select(int.Parse).ToArray();

            int customerId = commandArgs[0];
            Order order = new Order { CustomerId = customerId };

            for (int i = 1; i < commandArgs.Length; i++)
            {
                var itemId = commandArgs[i];

                order.Items.Add(new OrderItem { ItemId = itemId });
            }

            db.Add(order);
            db.SaveChanges();
        }

        private static void PrintSalesmenWithCustomerCount(ShopDbContext db)
        {
            var salesmenData = db.Salesmen
                .Select(s => new { s.Name, Customers = s.Customers.Count })
                .OrderByDescending(c => c.Customers)
                .ThenBy(c => c.Name);

            foreach (var item in salesmenData)
            {
                Console.WriteLine($"{item.Name} - {item.Customers} customers");
            }
        }

        private static void PrintCustomerOrdersAndReviewsCount(ShopDbContext db)
        {
            var customersData = db.Customers
                .Select(c => new
                {
                    c.Name,
                    Reviews = c.Reviews.Count,
                    Orders = c.Orders.Count
                })
                .OrderByDescending(c => c.Orders)
                .ThenByDescending(c => c.Reviews);

            foreach (var customer in customersData)
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine($"Orders: {customer.Orders}");
                Console.WriteLine($"Reviews: {customer.Reviews}");
            }
        }

        private static void PrintOrderItemAndReviewsCountByCustomerId(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = db.Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    Orders = c.Orders.Select(o => new
                    {
                        o.Id,
                        Items = o.Items.Count
                    })
                                .OrderBy(o => o.Id),
                    Reviews = c.Reviews.Count
                })
                .FirstOrDefault();

            foreach (var order in customerData.Orders)
            {
                Console.WriteLine($"order {order.Id}: {order.Items} items");
            }

            Console.WriteLine($"reviews: {customerData.Reviews}");
        }

        private static void PrintCustomerDataById(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = db.Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    c.Name,
                    Orders = c.Orders.Count,
                    Reviews = c.Reviews.Count,
                    Salesman = c.Salesman.Name
                })
                .FirstOrDefault();

            Console.WriteLine($"Customer: {customerData.Name}");
            Console.WriteLine($"Orders count: {customerData.Orders}");
            Console.WriteLine($"Reviews count: {customerData.Reviews}");
            Console.WriteLine($"Salesman: {customerData.Salesman}");
        }

        private static void PrintOrdersCountByCustomerId(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var ordersCount = db.Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o => o.Items.Count > 1)
                .Count();

            Console.WriteLine($"Orders: {ordersCount}");
        }
    }
}
