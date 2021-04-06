using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Linq;
using System.Text;

namespace RealEstates.ConsoleAppication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var db = new RealEstateDbContext();
            db.Database.Migrate();

            IPropertiesService propertiesService = new PropertiesService(db);
            var properties = propertiesService.SearchByPrice(0, 200000);

            foreach (var property in properties.Take(3))
            {
                Console.WriteLine($"District: {property.District}, Floor: {property.Floor},Size: {property.Size}, Year: {property.Year}, {property.BuildingType}, {property.PropertyType}");
            }

            //propertiesService.Create(2000, 3, 10, "Manastirski livadi", "mesonet", "brich", 2020, 5000000);
            //propertiesService.UpdateTags(1);

            //IDistrictsService districtService = new DistrictService(db);
            //var districts = districtService.GetTopDistrictsByAveragePrice();

            //foreach (var district in districts)
            //{
            //    Console.WriteLine($"{district.Name} - {district.AveragePrice} ,{district.MinPrice}, {district.MaxPrice}");

            //}
        }
    }
}
