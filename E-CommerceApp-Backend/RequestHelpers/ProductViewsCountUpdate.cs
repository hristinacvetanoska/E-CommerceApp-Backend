using E_CommerceApp_Backend.Models;

namespace E_CommerceApp_Backend.RequestHelpers
{
    public class ProductViewsCountUpdate
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductViewsCountUpdate(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ResetViewCounter()
        {

            await ResetViewCounterInDatabase();

        }


        private async Task ResetViewCounterInDatabase()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceContext>();


                var products = dbContext.Products.ToList();
                foreach (var product in products)
                {
                    product.ViewsCounter = 0;
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
