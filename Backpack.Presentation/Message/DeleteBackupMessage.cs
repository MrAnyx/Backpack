using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.Generic;

namespace Backpack.Presentation.Message;

public class DeleteBackupMessage(IEnumerable<Domain.Entity.Backup> backups) : ValueChangedMessage<IEnumerable<Domain.Entity.Backup>>(backups)
{
}
