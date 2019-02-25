using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public interface IViewModel
    {
        Task LoadAsync(int id = 0);
        Task ReloadAsync();
        bool IsLoaded { get; set; }
    }
}