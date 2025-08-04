using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Backup;

public class NewBackupCommand : ICommand<Domain.Entity.Backup>
{
    public required Domain.Entity.Backup Backup { get; init; }
}

public class NewBackupHandler(
    IBackupRepository _backupRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<NewBackupCommand, Domain.Entity.Backup>
{
    public async Task<Result<Domain.Entity.Backup>> HandleAsync(NewBackupCommand command, RequestContext context, CancellationToken cancellationToken)
    {
        var newBackup = _backupRepository.Add(command.Backup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newBackup;
    }
}
