using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RavenDB.AspNet.Identity;
using System.ComponentModel;

namespace MvcPWy.Models
{
    public interface IIndex
    {
        string Id { get; set; }
    }

    public class Project : IIndex
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [HitList(true)]
        public string Code { get; set; }
        [Required]
        [HitList(true)]
        public string Name { get; set; }
        public State State { get; set; }
        [Display(Name ="sqm")]
        [HitList(true, true, true)]
        public int Sqm { get; set; }
        [Display(Name ="Project type")]
        public ProjectType[] ProjectTypes { get; set; }

        [HitList(true, false)]
        public string Country { get; set; }
        [HitList(true, false)]
        public string Region { get; set; }
        [HitList(true, false)]
        public string Province { get; set; }
        [HitList(true, false)]
        public string Town { get; set; }

        [HitList(true, false, false)]
        public bool Offer { get; set; }
        [HitList(true, false, false)]
        public bool Order { get; set; }
        [HitList(true, false, false)]
        public bool Claim { get; set; }

        [DataType(DataType.Date)]
        [HitList(true, false)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        [HitList(true, false)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Updated { get; set; }

        Contact[] Contacts { get; set; }

    }

    public enum State
    {
        Open,
        Close
    }

    public class ProjectType
    {
        public string Type { get; set; }
        public ProjectTypeDescription[] Descriptions { get; set; }
    }

    public class ProjectTypeDescription
    {
        public string ProjectType { get; set; }
        public string Description { get; set; }
    }
}