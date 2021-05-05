using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IProductService
    {


        void AddProduct(AddProductInputServiceModel model);

        ICollection<ListAllProductsByProductTypeServiceModel> ListAllByProductType(string type);

        ProductDetailsServiceModel GetById(string id);

        ICollection<ListAllProductsServiceModel> GetAll();

        ICollection<ListAllProductByNameServiceModel> SearchByName(string searchStr, bool caseSensitive);

        bool RemoveById(string id);

        bool RemoveByName(string name);

        void EditModel(string id, EditProductInputServiceModel model);
    }
}
