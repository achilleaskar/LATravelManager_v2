using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using static LaWeb.Shared.Helpers.JsonBool;

namespace LATravelManager.Model.BookingData
{
    public class BookingForSearchDto
    {
        public List<ReservationFullDto> Ress { get; set; }
        [JsonProperty(PropertyName = "rd")]
        public decimal Recieved { get; set; }
        [JsonProperty(PropertyName = "rm")]
        public decimal Remaining { get; set; }
        [JsonProperty(PropertyName = "pn")]
        public string PartnerName { get; set; }
        [JsonConverter(typeof(NumericStringToBooleanConverter))]
        [JsonProperty(PropertyName = "rt")]
        public bool Reciept { get; set; }
        [JsonProperty(PropertyName = "u")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "cr")]
        public string CancelReason { get; set; }
        [JsonProperty(PropertyName = "cd")]
        public DateTime? CancelDate { get; set; }
        [JsonProperty(PropertyName = "co")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "d")]
        public string Destination { get; set; }
        [JsonProperty(PropertyName = "i")]
        public int Id { get; set; }
        [JsonConverter(typeof(NumericStringToBooleanConverter))]
        [JsonProperty(PropertyName = "sd")]
        public bool SecondDepart { get; set; }
        [JsonProperty(PropertyName = "et")]
        public ExcursionTypeEnum ExcursionType { get; set; }
        [JsonProperty(PropertyName = "ul")]
        public int UserLocation { get; set; }
        [JsonConverter(typeof(NumericStringToBooleanConverter))]
        [JsonProperty(PropertyName = "g")]
        public bool IsGroup { get; set; }
        [JsonProperty(PropertyName = "ei")]
        public int ExcursionId { get; set; }
        [JsonConverter(typeof(NumericStringToBooleanConverter))]
        [JsonProperty(PropertyName = "b")]
        public bool Bank { get; set; }
    }
}