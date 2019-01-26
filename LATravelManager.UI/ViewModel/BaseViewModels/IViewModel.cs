using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public interface IViewModel
    {
       Task LoadAsync();
    }
}