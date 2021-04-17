using AutoMapper;
using AutoMapper.QueryableExtensions;

using PetStore.Common;
using PetStore.Data;
using PetStore.Models;
using PetStore.Models.Enumerations;
using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services
{
    public class ProductService : IProductService
    {
        private readonly PetStoreDbContext dbContext;
        private readonly IMapper mapper;
        public ProductService(PetStoreDbContext petStoreDbContext, IMapper mapper)
        {
            this.dbContext = petStoreDbContext;
            this.mapper = mapper;
        }

        public void AddProduct(AddProductInputServiceModel model)
        {
            try
            {
                Product product = this.mapper.Map<Product>(model);
                this.dbContext.Products.Add(product);
                this.dbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }

        }

        public ICollection<ListAllProductsServiceModel> GetAll()
        {
            var products = this.dbContext
                .Products
                .ProjectTo<ListAllProductsServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return products;
        }

        public ICollection<ListAllProductByNameServiceModel> SearchByName(string searchStr, bool caseSensitive)
        {
            ICollection<ListAllProductByNameServiceModel> products;

            if (caseSensitive)
            {
                products = this.dbContext
              .Products
              .Where(p => p.Name.Contains(searchStr))
              .ProjectTo<ListAllProductByNameServiceModel>(this.mapper.ConfigurationProvider)
              .ToList();
            }
            else
            {
                products = this.dbContext
              .Products
              .Where(p => p.Name.ToLower().Contains(searchStr.ToLower()))
              .ProjectTo<ListAllProductByNameServiceModel>(this.mapper.ConfigurationProvider)
              .ToList();
            }

            return products;

        }

        public ICollection<ListAllProductsByProductTypeServiceModel> ListAllByProductType(string type)
        {
            ProductType productType;

            bool hasParced = Enum.TryParse(type, true, out productType);

            if (!hasParced)
            {
                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }

            var productsServiceModels = this.dbContext
                .Products
                .Where(p => p.ProductType == productType)
                .ProjectTo<ListAllProductsByProductTypeServiceModel>
                (this.mapper.ConfigurationProvider)
                .ToList();

            return productsServiceModels;
        }

        public bool RemoveById(string id)
        {
            Product productToRemove = this.dbContext
                .Products
                .Find(id);

            if (productToRemove == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductNotFound);
            }

            this.dbContext.Remove(productToRemove);
            int rowsAffected = this.dbContext.SaveChanges();
            bool wasDeleted = rowsAffected == 1;

            return wasDeleted;
        }

        public bool RemoveByName(string name)
        {
            Product productToRemove = this.dbContext.Products.FirstOrDefault(p => p.Name == name);

            if (productToRemove == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductWithGivenNameNotFound);
            }

            this.dbContext.Remove(productToRemove);
            int rowsAffected = this.dbContext.SaveChanges();

            return rowsAffected != 0;



        }

        public void EditModel(string id, EditProductInputServiceModel model)
        {

            try
            {
                Product product = this.mapper.Map<Product>(model);

                Product productToUpdate = this.dbContext
                    .Products
                    .Find(id);
                if (productToUpdate == null)
                {

                    throw new ArgumentException(ExceptionMessages.ProductNotFound);

                }

                productToUpdate.Name = product.Name;
                productToUpdate.ProductType = product.ProductType;
                productToUpdate.Price = product.Price;

                this.dbContext.SaveChanges();


            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception)
            {

                throw new ArgumentException(ExceptionMessages.InvalidProductType);
            }
        }
    }
}
