using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Runtime.InteropServices;
using System.Threading;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqConnectionMonitor : IHostedService, IDisposable, IRabbitMqConnectionMonitor
{
    private Timer? _timer;
    private IConnection? _connection;
    private ConnectionFactory _factory;
    private IChannel? _publishChannel;
    private RabbitMqConfiguration _configuration;
    private ILogger<IRabbitMqConnectionMonitor> _logger;
    private CancellationTokenSource? _cancellationTokenSource;
    private const int Period = 3;
    private bool _isInitialized = false;
    private readonly object _lock = new object();

    public RabbitMqConnectionMonitor(RabbitMqConfiguration configuration, ILogger<RabbitMqConnectionMonitor> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _factory = new ConnectionFactory
        {
            HostName = _configuration.HostName,
            UserName = _configuration.UserName,
            Password = _configuration.Password,
            Port = configuration.Port
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _ = Task.Run(() => RunPeriodicCheckAsync(_cancellationTokenSource.Token));
        return Task.CompletedTask;
    }

    private async Task RunPeriodicCheckAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Period));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                if (await CheckConnectionAsync() && !_isInitialized)
                {
                    _isInitialized = await TryInitializeAsync();
                }
                await timer.WaitForNextTickAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task<bool> TryInitializeAsync()
    {
        try
        {
            await using var channel = await _connection!.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(_configuration.Exchange, "topic");

            foreach (var q in _configuration.Queues)
            {
                await channel.QueueDeclareAsync(q.Name, durable: true, exclusive: false, autoDelete: false);
                await channel.QueueBindAsync(q.Name, _configuration.Exchange, q.RoutingKey);
            }
        } 
        catch
        {
            _logger.LogWarning("Не удалось инициализировать");
        }

        return true;
    }

    private async Task<bool> CheckConnectionAsync()
    {
        try
        {
            if (_connection == null || !_connection.IsOpen)
            { 
            
                _connection = await _factory.CreateConnectionAsync();
                _publishChannel = await _connection.CreateChannelAsync();

                _logger.LogInformation("Соединение с RabbitMq восстановлено");
            }
        }
        catch (BrokerUnreachableException)
        {
            _connection = null;
            _publishChannel = null;
            _logger.LogWarning($"RabbitMQ недоступен, повтор через {Period} с...");
            return false;
        }

        return true;
    }

    public IConnection? Connection
    {
        get
        {
            lock (_lock)
            {
                return _isInitialized && _connection is not null && _connection.IsOpen ? _connection : null;
            }
        }
    }

    public IChannel? PublishChannel
    {
        get
        {
            lock (_lock)
            {
                return _isInitialized && _connection is not null && _connection.IsOpen ? _publishChannel : null;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        _publishChannel?.Dispose();
        _connection?.Dispose();
        return Task.CompletedTask;
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }
}