using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {

          
            Mapper.Initialize(x =>
            {
                x.AddProfile<ProductShopProfile>();
            });

           // var userXml = File.ReadAllText(@"C:\Users\usmenzabanov\Desktop\C#\Databases Advanced\ProductShop1\ProductShop\Datasets\categories-products.xml");

            using (ProductShopContext context = new ProductShopContext()) 
            {
               
                var result = GetUsersWithProducts(context);
               
                File.WriteAllText("../../../result.xml", result);
               

            }

        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any())
                .Select(x => new ExportUserAndProductDto()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new SoldProductDto()
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold.Select(p => new ProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(c=>c.Price)
                        .ToArray()


                    }

                })
                .OrderByDescending(x=>x.SoldProducts.Count)
                .ToArray();

            var customExport = new ExportCustomUserDto()
            {
                Count = users.Length,
                Users = users
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCustomUserDto),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[]
             {

                XmlQualifiedName.Empty
            });


            xmlSerializer.Serialize(new StringWriter(sb), customExport,namespaces);

            return sb.ToString().TrimEnd();
               

        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.Select(x =>
              new GetCategoryProductsDto()
              {
                  Name = x.Name,
                  Count = x.CategoryProducts.Count,
                  AveragePrice = x.CategoryProducts.Sum(cp => cp.Product.Price) / x.CategoryProducts.Count,
                  TotalRevenue = x.CategoryProducts.Sum(cp => cp.Product.Price)

              })
            .OrderByDescending(x=>x.Count)
            .ThenBy(x=>x.TotalRevenue)
            .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GetCategoryProductsDto[]),
                new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[]
              {

                XmlQualifiedName.Empty
            });

            xmlSerializer.Serialize(new StringWriter(sb), categories,namespaces);

            return sb.ToString().TrimEnd();
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(x => x.ProductsSold.Any())
                .Select(x => new ExportUserSoldProductDto()
                {
                    FirstName=x.FirstName,
                    LastName=x.LastName,
                    ProductDto=Mapper.Map<ProductDto[]>(x.ProductsSold)
                    /*x.ProductsSold.Select(p => new ProductDto()
                    {
                        Name=p.Name,
                        Price=p.Price

                    }).ToArray()*/

                })
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Take(5)
            .ToArray();
            XmlSerializer serializer = new XmlSerializer(typeof(ExportUserSoldProductDto[]),
               new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces(new[]
            {

                XmlQualifiedName.Empty
            });

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), products,namespaces);

            return sb.ToString().TrimEnd();



        }
        public static string ImportCategoryProducts(ProductShopContext context,string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDto[]),
                new XmlRootAttribute("CategoryProducts"));

            var serializedCategories = (ImportCategoryProductDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var categoryProducts = new List<CategoryProduct>();

            foreach (var categoryDto in serializedCategories)
            {
                var product = context.Products.Find(categoryDto.ProductId);
                var category = context.Categories.Find(categoryDto.CategoryId);

                if (product == null || category == null) 
                {
                    continue;
                }

                var categotyProduct = Mapper.Map<CategoryProduct>(categoryDto);
                categoryProducts.Add(categotyProduct);
            }
     
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();


            return $"Successfully imported {categoryProducts.Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryDto[]),
                new XmlRootAttribute("Categories"));

            var serializedCategories = (ImportCategoryDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));

            var categories = new List<Category>();

            foreach (var categoryDto in serializedCategories)
            {
                var category = Mapper.Map<Category>(categoryDto);
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();


            return $"Successfully imported {categories.Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProductDto[]), 
                new XmlRootAttribute("Products"));

            var products = (ImportProductDto[])xmlSerializer
                .Deserialize(new StringReader(inputXml));               

                
            var resultProducts = new List<Product>();
            foreach (var product in products.Where(x => x.BuyerId != 0 && x.SellerId != 0))
            {
                var mappedProduct = Mapper.Map<Product>(product);

                resultProducts.Add(mappedProduct);
            }

            context.AddRange(resultProducts);
            context.SaveChanges();

            return $"Successfully imported {resultProducts.Count}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000 )
                .Select(x => new ExportProductInRangeDto
                {
                    Name = x.Name,
                    Price = x.Price,
                    BuyerName=x.Buyer.FirstName + " " + x.Buyer.LastName



                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportProductInRangeDto[]), 
                new XmlRootAttribute("Products"));

            StringBuilder sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb),products);

            return sb.ToString().TrimEnd();

            
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[])
                ,new XmlRootAttribute("Users"));
            var resultUsers = new List<User>();

            using (StringReader stringReader = new StringReader(inputXml)) 
            {
                var users = (ImportUserDto[])serializer.Deserialize(stringReader);
                foreach (var userDto in users)
                {
                    var user = Mapper.Map<User>(userDto);

                    resultUsers.Add(user);

                }
            }


           
            context.Users.AddRange(resultUsers);
            context.SaveChanges();

            return $"Successfully Imported {resultUsers.Count}";


        }
    }
}