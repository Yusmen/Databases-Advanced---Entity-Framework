using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RealEstates.Services
{
    public class PropertiesService : IPropertiesService
    {
        private RealEstateDbContext db;

        public PropertiesService(RealEstateDbContext db)
        {
            this.db = db;

        }

        public void Create(int size, int floor, int maxFloors, string district, string propertyType, string buildingType, int? year, int price)
        {
            var property = new RealEstateProperty
            {
                Size = size,
                Price = price,
                Year = year,
                Floor = floor,
                TotalNumberOfFloors = maxFloors


            };
            if (year < 1800)
            {
                property.Year = null;

            }
            if (floor <= 0)
            {
                property.Floor = null;
            }
            if (maxFloors <= 0)
            {
                property.TotalNumberOfFloors = null;
            }

            var districtEntity = this.db.Districts
                .FirstOrDefault(x => x.Name.Trim() == district.Trim());

            if (districtEntity == null)
            {
                districtEntity = new District
                {
                    Name = district
                };
            }
            property.District = districtEntity;

            var buildingTypeEntity = this.db.BuildingTypes
                .FirstOrDefault(x => x.Name.Trim() == buildingType.Trim());

            if (buildingTypeEntity == null)
            {
                buildingTypeEntity = new BuildingType
                {
                    Name = buildingType

                };
            }
            property.BuildingType = buildingTypeEntity;

            var propertyTypeEntity = this.db.PropertyTypes
                .FirstOrDefault(x => x.Name.Trim() == propertyType.Trim());

            if (propertyTypeEntity == null)
            {
                propertyTypeEntity = new PropertyType
                {
                    Name = propertyType
                };
            }

            property.PropertyType = propertyTypeEntity;
            //TODO: Tags
            db.RealEstateProperties.Add(property);
            db.SaveChanges();
            this.UpdateTags(property.Id);
        }


        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return db.RealEstateProperties
             .Where(x => x.Year >= minYear && x.Year <= maxYear && x.Size <= maxSize)
             .Select(MapToPropertyViewModel())
             .OrderBy(x => x.Price)
             .ToList();
        }

        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToPropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                Price = x.Price,
                Floor = (x.Floor ?? 0) + "/" + (x.TotalNumberOfFloors ?? 0),
                Size = x.Size,
                Year = x.Year,
                BuildingType = x.BuildingType.Name,
                District = x.District.Name,
                PropertyType = x.PropertyType.Name
            };
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.RealEstateProperties
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(MapToPropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }

        public void UpdateTags(int properyId)
        {

            var property = db.RealEstateProperties.FirstOrDefault(x => x.Id == properyId);
            property.Tags.Clear();
            if (property.Year.HasValue && property.Year < 1900)
            {
                property.Tags.Add(new RealEstatePropertyTag { Tag = this.GetOrCreateTag("OldBuilding") });
            }

        }
        private Tag GetOrCreateTag(string tagName)
        {
            var tag = this.db.Tags.FirstOrDefault(x => x.Name.Trim() == tagName.Trim());
            if (tag == null)
            {
                tag = new Tag { Name = tagName };
            }
            return tag;
        }

    }
}
