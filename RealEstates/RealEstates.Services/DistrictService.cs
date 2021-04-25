using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RealEstates.Services
{
    public class DistrictService : IDistrictsService
    {
        private RealEstateDbContext db;

        public DistrictService(RealEstateDbContext db)
        {
            this.db = db;
        }



        public IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePrice(int count = 10)
        {
            return this.db.Districts
                .OrderByDescending(x => x.Properties.Average(p => p.Price))
                .Select(MapToDistrictViewModel())
                .Take(count)
                .ToList();
        }

        private static Expression<Func<District, DistrictViewModel>> MapToDistrictViewModel()
        {
            return x => new DistrictViewModel
            {
                Name = x.Name,
                AveragePrice = x.Properties.Average(p => p.Price),
                MinPrice = x.Properties.Min(y => y.Price),
                MaxPrice = x.Properties.Max(y => y.Price),
                PropertiesCount = x.Properties.Count

            };
        }

        public IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count = 10)
        {
            return this.db.Districts
                .OrderByDescending(x => x.Properties.Count)
                .Select(MapToDistrictViewModel())
                .Take(count)
                .ToList();
        }
    }
}
