using System;
using System.Threading.Tasks;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Settings_Viewmodel : MyViewModelBaseAsync
    {
        public Settings_Viewmodel(Window_ViewModels.MainViewModel mainViewModel)
        {
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            try
            {
                await Task.Delay(0);
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}