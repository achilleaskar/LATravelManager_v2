﻿using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class Settings_Viewmodel : MyViewModelBaseAsync
    {
        public Settings_Viewmodel()
        {
        }

        public override Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
                throw new NotImplementedException();
            }
        }
        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}