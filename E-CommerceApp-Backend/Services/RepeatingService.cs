using E_CommerceApp_Backend.RequestHelpers;

namespace E_CommerceApp_Backend.Services
{
    public class RepeatingService : BackgroundService
    {
        private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromHours(24));
        private readonly ILogger<RepeatingService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public RepeatingService(ILogger<RepeatingService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(await _periodicTimer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await UpdateFieldsInDatabase();
            }
        }


        private async Task UpdateFieldsInDatabase()
        {
            ProductViewsCountUpdate productViewsCountUpdate = new ProductViewsCountUpdate(_scopeFactory);
            productViewsCountUpdate.ResetViewCounter();
        }
    }
}
