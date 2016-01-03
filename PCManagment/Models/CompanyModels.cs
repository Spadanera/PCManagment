using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcPWy.Models
{
    public class Company : IContact
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        [Display(Name = "Document sent")]
        public bool DocumentSent { get; set; }
        [Display(Name = "Notified to distributor")]
        public bool NotifiedToDistributor { get; set; }
        public bool Handled { get; set; }
        [Display(Name = "Contact type")]
        public ContactType ContactType { get; set; }
        [Editable(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Added { get; set; }
        [Editable(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Updated { get; set; }
        public ContactDetail[] Details { get; set; }
        public Warning[] Warnings { get; set; }

        Contact[] Contacts { get; set; }
    }
}