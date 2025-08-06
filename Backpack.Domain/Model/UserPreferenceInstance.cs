using System.Globalization;

namespace Backpack.Domain.Model;

public class UserPreferenceInstance
{
    public required CultureInfo Culture { get; set; }
}
