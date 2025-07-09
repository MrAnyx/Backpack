using Backpack.Domain.Entity;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Backpack.Presentation.Message;

public class NewSuccessfulBackupMessage(Backup backup) : ValueChangedMessage<Backup>(backup) { }
