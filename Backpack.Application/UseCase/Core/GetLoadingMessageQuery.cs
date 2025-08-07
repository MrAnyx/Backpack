using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Core;

public class GetLoadingMessageQuery : IQuery<string>
{
}

public class GetLoadingMessageHandler : IQueryHandler<GetLoadingMessageQuery, string>
{
    private IEnumerable<string> _loadingMessages => [
            "Still faster than your morning coffee...",
            "Loading... bribing the hamsters to run faster.",
            "Hold on, aligning the stars...",
            "Downloading data from the cloud (hope it's not raining).",
    ];

    public Task<Result<string>> HandleAsync(GetLoadingMessageQuery query, RequestContext context, CancellationToken cancellationToken)
    {
        var message = _loadingMessages.ElementAt(new Random().Next(0, _loadingMessages.Count()));
        return Task.FromResult(new Result<string>(message));
    }
}
