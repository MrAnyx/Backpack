using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Backpack.Presentation.Message;

public class UpdateBackupMessage(Domain.Entity.Backup backup) : ValueChangedMessage<Domain.Entity.Backup>(backup)
{
}
