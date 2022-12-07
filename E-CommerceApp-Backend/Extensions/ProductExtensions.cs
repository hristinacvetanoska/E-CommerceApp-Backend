using E_CommerceApp_Backend.Models;

namespace E_CommerceApp_Backend.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy)) return query.OrderBy(p => p.Name);

            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name),
            };

            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string? brands, string? types, string? sellerName)
        {
            var brandList = new List<string>();
            var typeList = new List<string>();
            var sellerList = new List<string>();

            if (!string.IsNullOrEmpty(brands))
                brandList.AddRange(brands.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(types))
                typeList.AddRange(types.ToLower().Split(",").ToList());

            if(!string.IsNullOrEmpty(sellerName))
                sellerList.AddRange(sellerName.ToLower().Split(",").ToList());


            query = (brandList.Count == 0) ? query : query.Where(p => brandList.Contains(p.Brand.ToLower()));
            query = (typeList.Count == 0) ? query : query.Where(p => typeList.Contains(p.Type.ToLower()));
            query = (sellerList.Count == 0) ? query : query.Where(p => sellerList.Contains(p.SellerName.ToLower()));

            return query;
        }
    }
}
