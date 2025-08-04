using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Backpack.Presentation.Message;

public class DeleteBackupMessage(Domain.Entity.Backup backup) : ValueChangedMessage<Domain.Entity.Backup>(backup)
{
}
