using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Website.Models.FilteringModels
{
    
    public enum FaultPreviewOrder
    {
        [Display(Name = "Ostatnie zgłoszenie")]
        LastFault,
        [Display(Name = "Producent")]
        Manufacturer,
        [Display(Name = "Model")]
        Model,
        [Display(Name ="Numer rejestracyjny")]
        RegisterNumber
    }
    public class CustomerFaultsPreviewModel
    {
        public IEnumerable<RepairStatusViewModel> Cars { get; set; }
        [DisplayName("Producent")]
        public string Manufacturer { get; set; }
        [DisplayName("Model")]
        public string Model { get; set; }
        [DisplayName("Numer rejestracyjny")]
        public string RegisterNumber { get; set; }

        [DisplayName("Sortuj według")]
        public FaultPreviewOrder Order { get; set; }

    }
}