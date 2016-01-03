using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcPWy.Models
{
    public interface IContact
    {
        string Title { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Address { get; set; }
        string ZIP { get; set; }
        string City { get; set; }
        string Province { get; set; }
        string Region { get; set; }
        string Country { get; set; }
        [Display(Name = "Document sent")]
        bool DocumentSent { get; set; }
        [Display(Name = "Notified to distributor")]
        bool NotifiedToDistributor { get; set; }
        bool Handled { get; set; }
        [Display(Name = "Contact type")]
        ContactType ContactType { get; set; }
        [Editable(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime Added { get; set; }
        [Editable(false)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime Updated { get; set; }
        ContactDetail[] Details { get; set; }
        Warning[] Warnings { get; set; }
    }

    public class ContactType
    {
        public string Description { get; set; }
    }

    public class ContactDetail
    {
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

    public class Warning
    {
        string Contact { get; set; }
        string Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Due date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        DateTime DueDate { get; set; }
    }
}