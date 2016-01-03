using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RavenDB.AspNet.Identity;

namespace MvcPWy.Models
{
    public interface IContact
    {
        [Key]
        string Id { get; set; }
        string Address { get; set; }
        string ZIP { get; set; }
        string City { get; set; }
        string Province { get; set; }
        string Region { get; set; }
        string Country { get; set; }

        ContactType ContactType { get; set; }

        DateTime Added { get; set; }
        DateTime Updated { get; set; }

        List<ContactDetail> Details { get; set; }

        List<string> Projects { get; set; }
    }

    public class ContactType : IIndex
    {
        [Key]
        public string Id { get; set; }
        [HitList(true)]
        [Display(Name = "Contact Type Description")]
        public string Description { get; set; }
    }

    public class ContactDetail
    {
        [Key]
        public string Id { get; set; }
        string Contact { get; set; }
        ContactDetailType Type { get; set; }
        string Description { get; set; }
        string Value { get; set; }
    }

    public enum ContactDetailType
    {
        eMail,
        Fax,
        Mobile,
        Phone,
        Url,
        LinkedIn
    }

    public class Warning : IIndex
    {
        [Key]
        public string Id { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
    }
}