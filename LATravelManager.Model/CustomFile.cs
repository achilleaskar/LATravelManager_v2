using System.ComponentModel.DataAnnotations;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Model
{
    public class CustomFile : BaseModel
    {
        private string _FileName;

        [StringLength(255)]
        public string FileName
        {
            get
            {
                //if (string.IsNullOrEmpty(_FileName))
                //{
                //    return "Κενό";
                //}
                //if (_FileName.Length > 15)
                //{
                //    return _FileName.Substring(0, 15) + "...";
                //}
                return _FileName;
            }

            set
            {
                if (_FileName == value)
                {
                    return;
                }

                _FileName = value;
                RaisePropertyChanged();
            }
        }

        public byte[] Content { get; set; }
        public FileType FileType { get; set; }
    }
}