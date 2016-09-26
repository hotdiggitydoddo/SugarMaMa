using System.Collections.Generic;

namespace SugarMaMa.API.DAL.Entities
{
    public class Location : SMEntity<int>
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public List<BusinessDay> BusinessDays { get; set; }
    }
}
