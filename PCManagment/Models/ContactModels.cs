using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RavenDB.AspNet.Identity;

namespace MvcPWy.Models
{
    public class Contact : IIndex, IContact
    {
        [Key]
        public string Id { get; set; }
        public string CompanyId { get; set; }
        [HitList(true, true, false)]
        public string CompanyName { get; set; }
        public string Title { get; set; }
        [HitList(true)]
        public string Name { get; set; }
        [HitList(true)]
        public string Surname { get; set; }
        public string Address { get; set; }
        public string ZIP { get; set; }
        [HitList(true, false, true)]
        public string City { get; set; }
        [HitList(true, false, true)]
        public string Province { get; set; }
        [HitList(true, false, true)]
        public string Region { get; set; }
        [HitList(true, false, true)]
        public string Country { get; set; }

        [HitList(true, false, false)]
        [Display(Name="Document sent")]
        public bool DocumentSent { get; set; }
        [HitList(true, false, false)]
        [Display(Name = "Notified to distributor")]
        public bool NotifiedToDistributor { get; set; }
        [HitList(true, false, false)]
        public bool Handled { get; set; }
        [Display(Name ="Contact type")]
        public ContactType ContactType { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Added { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [HitList(true, false, true)]
        public DateTime Updated { get; set; }

        public List<ContactDetail> Details { get; set; }
        public Warning[] Warnings { get; set; }

        public List<string> Projects { get; set; }
    }
}
