using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace AutoServiceManager.Common.Model
{
   
    public class CataloguePart
    {
        public CataloguePart()
        {
            
        }

        public long Id { get; set; }

        [Required]
        [DisplayName("Nazwa części")]
        [MaxLength(200)]
        public string PartName { get; set; }

        [Required]
        [DisplayName("Numer seryjny")]
        [MaxLength(50)]
        [MinLength(5)]
        public string SerialNumber { get; set; }

        [Required]
        [DisplayName("Cena")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Ilość w magazynie")]
        public int InStock { get; set; }
    }
}