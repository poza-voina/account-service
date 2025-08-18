using AccountService.Api.ObjectStorage.Interfaces;
using AccountService.Api.ObjectStorage.Objects;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace AccountService.Api.ObjectStorage;

public class RabbitMqConnectionMonitor : IHostedService, IDisposable, IRabbitMqConnectionMonitor
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local Получает значение
    private Timer? _timer;
    private IConnection? _connection;
    private readonly ConnectionFactory _factory;
    private IChannel? _publishChannel;
    private readonly RabbitMqConfiguration _configuration;
    private readonly ILogger<IRabbitMqConnectionMonitor> _logger;
    private CancellationTokenSource? _cancellationTokenSource;
    private const int Period = 3;
    private bool _isInitialized;
    private readonly Lock _lock = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public RabbitMqConnectionMonitor(RabbitMqConfiguration configuration, ILogger<RabbitMqConnectionMonitor> logger, Timer? timer)
    {
        _configuration = configuration;
        _logger = logger;
        _timer = timer;

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
        _ = Task.Run(() => RunPeriodicCheckAsync(_cancellationTokenSource.Token), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task RunPeriodicCheckAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Period));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // ReSharper disable once InconsistentlySynchronizedField Читать могу без проблем
                if (!_isInitialized && await CheckConnectionAsync())
                {
                    await _semaphore.WaitAsync(cancellationToken);
                    try
                    {
                        // ReSharper disable once InconsistentlySynchronizedField Читать могу без проблем
                        if (!_isInitialized)
                        {
                            // ReSharper disable once InconsistentlySynchronizedField Lock Semaphore
                            _isInitialized = await TryInitializeAsync();
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
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
            _logger.LogCritical("Queue and exchange initialization failed");
            return false;
        }

        return true;
    }

    private async Task<bool> CheckConnectionAsync()
    {
        try
        {
            if (_connection is not { IsOpen: true })
            { 
            
                _connection = await _factory.CreateConnectionAsync();
                _publishChannel = await _connection.CreateChannelAsync();

                _logger.LogInformation("Connection open: {ConnectionIsOpen}", _connection?.IsOpen);
                _logger.LogInformation("Channel open: {PublishChannelIsOpen}", _publishChannel?.IsOpen);
            }
        }
        catch (BrokerUnreachableException)
        {
            _connection = null;
            _publishChannel = null;
            _logger.LogWarning("RabbitMQ is unavailable, retrying in {I}", Period);
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
                if (!_isInitialized || _connection is null || !_connection.IsOpen || _publishChannel?.IsOpen is false)
                    return null;

                return _publishChannel;
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