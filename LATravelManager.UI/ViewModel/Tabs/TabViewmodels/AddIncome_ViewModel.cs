using LATravelManager.Model;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;

namespace LATravelManager.UI.ViewModel.Tabs.TabViewmodels
{
    public class AddIncome_ViewModel : MyViewModelBase
    {
        public AddIncome_ViewModel()
        {
            Load();
        }
        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Transaction = new Transaction();
        }




        private Transaction _Transaction;


        public Transaction Transaction
        {
            get
            {
                return _Transaction;
            }

            set
            {
                if (_Transaction == value)
                {
                    return;
                }

                _Transaction = value;
                RaisePropertyChanged();
            }
        }
       

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}