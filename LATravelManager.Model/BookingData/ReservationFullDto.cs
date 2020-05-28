using System;
using Newtonsoft.Json;

namespace LATravelManager.Model.BookingData
{
    public class ReservationFullDto : BaseModel
    {
        [JsonProperty(PropertyName = "ci")]
        public DateTime CheckIn { get; set; }
        [JsonProperty(PropertyName = "co")]
        public DateTime CheckOut { get; set; }
        [JsonProperty(PropertyName = "n")]
        public string Names { get; set; }
        [JsonProperty(PropertyName = "d")]
        public string Departure { get; set; }
        [JsonProperty(PropertyName = "t")]
        public string Tel { get; set; }
        [JsonProperty(PropertyName = "a")]
        public DateTime Added { get; set; }
        [JsonProperty(PropertyName = "h")]
        public string Hotel { get; set; }
        [JsonProperty(PropertyName = "r")]
        public string RoomType { get; set; }
        [JsonProperty(PropertyName = "rt")]
        public ReservationTypeEnum ReservationType { get; set; }
        public BookingForSearchDto BookingForSearchDto { get; set; }
    }
}