﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoServiceManager.Common.Identity;

namespace AutoServiceManager.Common.Model
{
    [ComplexType]
    public class AddressData
    {
        [DisplayName("Miasto")]
        public long CityId { get; set; }

        [Required]
        [NotMapped]
        [DisplayName("Miasto")]
        public string TempCity { get; set; }

        [DisplayName("Kraj")]
        public long CountryId { get; set; }

        [EmailAddress]
        [DisplayName("Adres email")]
        public string Email { get; set; }

        [Phone]
        [DisplayName("Telefon")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Ulica")]
        public string Street { get; set; }

        [DisplayName("Kod pocztowy")]
        [RegularExpression(@"\d{2}\s*-\s*\d{3}", ErrorMessage = "Niepoprawny kod pocztowy")]
        public string ZipCode { get; set; }


    }


    public class Person
    {
        public Person()
        {
            Address = new AddressData();
        }

        public long ID { get; set; }
        [Required]
        [DisplayName("Imię")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Nazwisko")]
        public string SecondName { get; set; }     

        public AddressData Address { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
