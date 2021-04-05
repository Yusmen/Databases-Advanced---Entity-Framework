using RealEstates.Services.Models;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Create(int size, int floor, int maxFloors,
            string district, string propertyType, string buildingType, int? year, int price);

        void UpdateTags(int properyId);
        IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize);
        IEnumerable<PropertyViewModel> SearchByPrice(int minPrice,int maxPrice);
    }
}
