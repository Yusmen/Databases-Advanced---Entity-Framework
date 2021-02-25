using SalesDatabase.Data.Models;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagment.Contracts;
using System;
using System.Collections.Generic;

namespace SalesDatabase.Data.Seeding
{
    public class ProductSeeder : ISeeder
    {
        private readonly SalesContext dbContext;
        private readonly Random random;
        private readonly IWriter writer;
        public ProductSeeder(SalesContext context, Random random, IWriter writer)
        {
            this.dbContext = context;
            this.random = random;
            this.writer = writer;
        }
        public void Seed()
        {
            ICollection<Product> products = new List<Product>();


            string[] names = new string[]
            {
                "CPU",
                "MotherBoard",
                "GPU",
                "RAM",
                "SSD",
                "HDD",
                "CD-RW",
                "Air Cooler",
                "Water Cooler",
                "ThermoPaste"
            };

            for (int i = 0; i < 50; i++)
            {
                int nameIndex = this.random.Next(0, names.Length);
                string currentPrName = names[nameIndex];
                double quantity = this.random.Next(1000);
                decimal price = this.random.Next(5000) * 1.133m;

                Product product = new Product()
                {
                    Name = currentPrName,
                    Quantity = quantity,
                    Price = price

                };

                products.Add(product);
                this.writer
                    .WriteLine($"Product {currentPrName} {quantity} {price} was added to database");
            }

            this.dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

        }
    }
}
