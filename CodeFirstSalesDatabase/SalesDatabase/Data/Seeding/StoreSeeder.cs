using SalesDatabase.Data.Models;
using SalesDatabase.Data.Seeding.Contracts;
using SalesDatabase.IOManagment.Contracts;

namespace SalesDatabase.Data.Seeding
{
    public class StoreSeeder : ISeeder
    {

        private readonly SalesContext dbContext;
        private readonly IWriter writer;


        public StoreSeeder(SalesContext context,IWriter writer)
        {
            this.dbContext = context;
            this.writer = writer;
        }
        public void Seed()
        {
            Store[] stores = new Store[]
            {
                new Store()
                {
                    Name="PC-Tech Sofia"
                },
                new Store()
                {
                    Name="PC-Tech Plovdiv"
                },
                new Store()
                {
                    Name="PC-Tech Varna"
                },
                new Store()
                {
                    Name="Innovative Tech Sofia"
                },
                new Store()
                {
                    Name="Innovative Tech Plovdiv"
                }

            };

            this.dbContext.Stores.AddRange(stores);
            this.dbContext.SaveChanges();

            writer.WriteLine($"{stores.Length} stores were added to database");
        }
    }
}
