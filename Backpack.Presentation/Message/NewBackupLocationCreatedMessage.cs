using Backpack.Domain.Entity;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Backpack.Presentation.Message;

public class NewBackupLocationCreatedMessage(Backup backup) : ValueChangedMessage<Backup>(backup) { }
