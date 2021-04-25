using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class Reciept:RecieptBase
    {
        public byte[] Content { get; set; }
    }
}