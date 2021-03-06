﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceManager.Common.Model
{
    public class Part
    {
        public long Id { get; set; }
        [Required]
        [DisplayName("Numer katalogowy")]
        public long CataloguePartId { get; set; }
        public virtual CataloguePart PartFromCatalogue { get; set; }
        [Required]
        [DisplayName("Cena sprzedaży")]
        public decimal SoldPrice { get; set; }
        [Required]
        [DisplayName("Ilość")]
        public int Count { get; set; }
        [DisplayName("Całkowity Koszt")]
        public decimal Cost
        {
            get { return SoldPrice*Count; }
        }
    }
}