using System;

namespace LATravelManager.Model
{
    public class EditTracker : BaseModel
    {
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
