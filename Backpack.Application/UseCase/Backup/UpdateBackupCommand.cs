using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.UseCase.Backup;

public class UpdateBackupCommand : ICommand<Domain.Entity.Backup>
{
    public required Domain.Entity.Backup Backup { get; init; }
}

public class UpdateBackupHandler(
    IBackupRepository _backupRepository,
    IUnitOfWork _unitOfWork
) : ICommandHandler<UpdateBackupCommand, Domain.Entity.Backup>
{
    public async Task<Result<Domain.Entity.Backup>> HandleAsync(UpdateBackupCommand command, RequestContext context, CancellationToken cancellationToken)
    {
        var newBackup = _backupRepository.Update(command.Backup);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newBackup;
    }
}
