namespace LATravelManager.Model.Services
{
    public class GuideService : Service
    {
        public GuideService()
        {
            Title = "Ξενάγηση";
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

        public override string GetDescription()
        {
            return Title+" σε "+ From ?? "";
        }
    }
}