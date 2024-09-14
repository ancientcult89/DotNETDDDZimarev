
using EventStore.ClientAPI;

namespace Marketplace
{
    public class HostedService : IHostedService
    {
        private readonly IEventStoreConnection _connection;
        public HostedService(IEventStoreConnection connection)
        {
            _connection = connection;
        }
        public Task StartAsync(CancellationToken cancellationToken) => _connection.ConnectAsync();

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}
