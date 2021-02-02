namespace LATravelManager.Model.DTOS
{
    public class CompanyDto : BaseModel
    {
        private string _CompanyName;

        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }

            set
            {
                if (_CompanyName == value)
                {
                    return;
                }

                _CompanyName = value;
                RaisePropertyChanged();
            }
        }
    }
}