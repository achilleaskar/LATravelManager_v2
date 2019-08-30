namespace LATravelManager.UI.ViewModel.BaseViewModels
{
    public interface IViewModel
    {
        void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        void Reload();

        bool IsLoaded { get; set; }
    }
}