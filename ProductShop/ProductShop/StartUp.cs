using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dtos;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();



            //var userJson = File.ReadAllText(@"C:\Users\usmenzabanov\Desktop\C#\Databases Advanced\ProductShop\ProductShop\Datasets\categories-products.json");

            var result = GetUsersWithProducts(context);
            File.WriteAllText("../../../result2.json", result);






        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var products = context.Users.Where(x => x.ProductsSold.Any(p => p.Buyer != null))
                .OrderByDescending(x => x.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(x => new
                {
                    x.LastName,
                    x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Count(ps => ps.Buyer != null),
                        Products = x.ProductsSold
                        .Where(c => c.Buyer != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price

                        })
                    .ToList()
                    }

                })
                .ToList();

            var result = new
            {
                UsersCount = products.Count,
                Products = products
            };
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {

                NamingStrategy = new CamelCaseNamingStrategy()

            };
            var serialized = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });


            return serialized;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var products = context.Categories.Select(x => new
            {
                x.Name,
                x.CategoryProducts.Count,
                AveragePrice = x.CategoryProducts.Sum(p => p.Product.Price) / x.CategoryProducts.Count,
                TotalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)



            })
            .OrderByDescending(x => x.Count)
            .ToList();


            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {

                NamingStrategy = new CamelCaseNamingStrategy()

            };
            var serialized = JsonConvert.SerializeObject(products, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return serialized;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users.Where(x => x.ProductsBought.Any(p => p.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    SoldProducts = x.ProductsSold
                    .Where(c => c.Buyer != null)
                    .Select(pb => new
                    {
                        pb.Name,
                        pb.Price,
                        BuyerFirstName = pb.Buyer.FirstName,
                        BuyerLastName = pb.Buyer.LastName


                    }).ToList()

                }).ToList();


            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(x => new ProductDto
                {

                    Name = x.Name,
                    Price = x.Price,
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}"

                })
                .OrderBy(x => x.Price)
                .ToList();

            var json = JsonConvert.SerializeObject(products);
            return json;
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var validCategoryIds =
                context.Categories.Select(c => c.Id)
                .ToList();



            var validProductIds =
                context.Products.Select(c => c.Id)
                .ToList();


            var catProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson)
                .ToList();

            var validCategoryProducts = new List<CategoryProduct>();

            foreach (var categoryProduct in catProducts)
            {
                bool isValid = validCategoryIds.Contains(categoryProduct.CategoryId) && validProductIds.Contains(categoryProduct.ProductId);

                if (isValid)
                {
                    validCategoryProducts.Add(categoryProduct);
                }
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported { validCategoryProducts.Count}";



        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //};

            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null)
                .ToList();
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported { categories.Count}";

        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson)
                .ToList();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported { products.Count}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson)
                .Where(u => u.LastName != null && u.LastName.Length >= 3)
                .ToList();


            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported { users.Count}";

        }
    }
}