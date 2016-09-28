using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.Models.SpaServices
{
    public class SpaServiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsQuickService { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Cost { get; set; }
        public bool IsUnisex { get; set; }
    }
}
