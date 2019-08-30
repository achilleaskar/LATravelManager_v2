namespace LATravelManager.Model.Services
{
    public class GuideService : Service
    {
        public GuideService()
        {
            Tittle = "Ξενάγηση";
        }

        private int _Duration;

        public int Duration
        {
            get
            {
                return _Duration;
            }

            set
            {
                if (_Duration == value)
                {
                    return;
                }

                _Duration = value;
                RaisePropertyChanged();
            }
        }
    }
}