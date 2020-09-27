using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class EmailModel
    {
        [Key]
        public int EmailId { get; set; }
        public string Address { get; set; }
        public DateTime? CollectionTime { get; set; }
        public bool? Status { get; set; }
        public bool? ShouldBeDeleted { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyModel Company { get; set; }
    }
}
