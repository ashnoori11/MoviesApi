using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;



public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        string? userName = string.Empty;

        _logger.LogInformation("CleanArchitecture Request: {Name} {@RequestDate} {@Requesttime} {@Request}",requestName, DateTime.Now.Date, DateTime.Now.TimeOfDay, request);
    }
}