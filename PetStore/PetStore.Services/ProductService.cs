using PetStore.Data;

namespace PetStore.Services
{
    public class ProductService
    {
        private readonly PetStoreDbContext dbContext;
        public ProductService(PetStoreDbContext petStoreDbContext)
        {
            this.dbContext = petStoreDbContext;
        }

        public void AddProduct()




    }
}
