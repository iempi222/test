using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using Test.Domain.Models.Account;
using Test.Domain.Models.Events;

namespace Test.Domain.Models.Location
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        [Required]
        public string LocationTitle { get; set; }
        public int? ParentId { get; set; }
        public bool IsDelete { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public ICollection<Location> Locations { get; set; }
        public ICollection<User> Users { get; set; }

        #endregion
    }
}
