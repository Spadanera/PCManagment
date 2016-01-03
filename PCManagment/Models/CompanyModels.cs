using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RavenDB.AspNet.Identity;

namespace MvcPWy.Models
{
    public interface ICompany
    {
        [Key]
        string Id { get; set; }
        string Name { get; set; }
        List<Contact> Contacts { get; set; }
    }

    public class Company : IIndex, ICompany, IContact
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Company")]
        [HitList(true)]
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; }

        public string Address { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public ContactType ContactType { get; set; }

        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }

        public List<ContactDetail> Details { get; set; }

        public List<string> Projects { get; set; }
    }

    public class Distributor : IIndex, ICompany, IContact
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Distributor")]
        [HitList(true)]
        public string Name { get; set; }
        public Contact DefaultContact { get; set; }
        public List<Contact> Contacts { get; set; }

        public string Address { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public ContactType ContactType { get; set; }

        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }

        public List<ContactDetail> Details { get; set; }

        public List<string> Projects { get; set; }
    }
}