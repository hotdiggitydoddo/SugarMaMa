using System.Collections.Generic;

namespace SugarMaMa.API.DAL.Entities
{
    public class Location : SMEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public List<BusinessDay> BusinessDays { get; set; }
    }
}
