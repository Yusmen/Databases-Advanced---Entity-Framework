using RealEstates.Data;
using System;

namespace RealEstates.ConsoleAppication
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RealEstateDbContext();
            db.Database.EnsureCreated();
        }
    }
}
