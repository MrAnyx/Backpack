using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Backup;

public class GetAllBackupsQuery : IQuery<IEnumerable<Domain.Entity.Backup>>
{
}

public class GetAllBackupsHandler(IBackupRepository _backupRepository) : IQueryHandler<GetAllBackupsQuery, IEnumerable<Domain.Entity.Backup>>
{
    public async Task<Result<IEnumerable<Domain.Entity.Backup>>> HandleAsync(GetAllBackupsQuery query, PipelineContext context, CancellationToken cancellationToken)
    {
        var backups = await _backupRepository.GetAllAsync(cancellationToken: cancellationToken);

        return new Result<IEnumerable<Domain.Entity.Backup>>(backups);
    }
}
