using SalesDatabase.Data;
using SalesDatabase.Data.Seeding;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagment;
using SalesDatabase.IOManagment.Contracts;
using System;
using System.Collections.Generic;

namespace SalesDatabase
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            //SalesContext dbContext = new SalesContext();

            //Random random = new Random();
            //IWriter consoleWriter = new ConsoleWriter();

            //ICollection<ISeeder> seeders = new List<ISeeder>();

            //seeders.Add(new ProductSeeder(dbContext, random,consoleWriter));
            //seeders.Add(new StoreSeeder(dbContext,consoleWriter));

            //foreach (ISeeder seeder in seeders)
            //{
            //    seeder.Seed();
            //}
        }
    }
}
