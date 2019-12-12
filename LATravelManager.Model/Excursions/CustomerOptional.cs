using LATravelManager.Model.Lists;
using LATravelManager.Model.People;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Excursions
{
    public class CustomerOptional : BaseModel
    {
        public Customer Customer { get; set; }

        public OptionalExcursion OptionalExcursion { get; set; }
        public string Note { get; set; }

        private Leader _Leader;

        public Leader Leader
        {
            get
            {
                return _Leader;
            }

            set
            {
                if (_Leader == value)
                {
                    return;
                }

                _Leader = value;
                RaisePropertyChanged();
            }
        }
    }
}