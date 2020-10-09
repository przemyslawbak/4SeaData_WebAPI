using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyId { get; set; }
        public int? IMO { get; set; } //csv
        public DateTime? DatatUpdate { get; set; } //csv
        public DateTime? EmailsUpdate { get; set; } //email repo
        public DateTime? VesselsUpdate { get; set; } //vsl repo
        public string Name { get; set; } //csv
        public string CareOf { get; set; } //csv
        public string Status { get; set; } //csv
        public string Roles { get; set; } //vsl repo
        public string AddressComplete { get; set; } //csv
        public string AddressCity { get; set; } //csv
        public string AddressCountry { get; set; } //csv
        public string Telephone { get; set; } //csv
        public string Facsimile { get; set; } //csv
        public string[] FleetTypes { get; set; } //vsl repo
        public string Website { get; set; } //csv
        public bool? WebsiteStatus { get; set; } //csv

        public List<EmailModel> EmailList { get; set; } //.Include(c => c.EmailList)
        public List<VesselModel> VesselOwner { get; set; } //.Include(c => c.VesselOwner)
        public List<VesselModel> VesselManager { get; set; } //.Include(c => c.VesselManager)

        public int OwnerId { get => CompanyId; }
        public int ManagerId { get => CompanyId; }
    }
}
