using System;

namespace LATravelManager.Model.Plan
{
    public class PlanDatetInfo : BaseModel
    {
        #region Fields

        private bool _IsSelected;

        #endregion Fields



        #region Properties

        public DateTime Date { get; set; }

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion Properties

        #region Methods

        public override string ToString() => Date.ToString("ddd").Substring(0, 2);

        #endregion Methods
    }
}