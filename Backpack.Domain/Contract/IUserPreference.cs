using Backpack.Domain.Model;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract;

public interface IUserPreference
{
    UserPreferenceInstance Default { get; set; }

    Task SaveAsync();
    Task LoadAsync();
}
