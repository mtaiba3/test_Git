using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{
    public static class Extensions
    {
        public static itemDto AsDto(this Item item)
        {
            return new itemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}