using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Backup;

public class DeleteBackupCommand : ICommand
{
    public required IEnumerable<Domain.Entity.Backup> Backups { get; init; }
}

public class DeleteBackupHandler(
    IBackupRepository _backupRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteBackupCommand>
{
    public async Task<Result> HandleAsync(DeleteBackupCommand command, RequestContext context, CancellationToken cancellationToken)
    {
        foreach (var backup in command.Backups)
        {
            await _backupRepository.RemoveByIdAsync(backup.Id);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result();
    }
}
