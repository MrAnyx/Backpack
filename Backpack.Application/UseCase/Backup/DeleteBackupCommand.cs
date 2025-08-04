using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Backup;

public class DeleteBackupCommand : ICommand
{
    public required Domain.Entity.Backup Backup { get; init; }
}

public class DeleteBackupHandler(
    IBackupRepository _backupRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<DeleteBackupCommand>
{
    public async Task<Result> HandleAsync(DeleteBackupCommand command, RequestContext context, CancellationToken cancellationToken)
    {
        await _backupRepository.RemoveByIdAsync(command.Backup.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result();
    }
}
