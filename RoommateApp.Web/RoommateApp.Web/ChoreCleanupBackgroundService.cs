using RoommateApp.Bll.Services;

namespace RoommateApp.Web
{
    public class ChoreCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ChoreCleanupBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunCleanupAsync();

            using var timer = new PeriodicTimer(TimeSpan.FromHours(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunCleanupAsync();
            }
        }

        private async Task RunCleanupAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var apartmentService = scope.ServiceProvider.GetRequiredService<ApartmentService>();
                await apartmentService.CleanupExpiredChoresAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during scheduled chore cleanup: {ex.Message}");
            }
        }
    }
}