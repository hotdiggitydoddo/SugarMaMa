using System;

namespace SugarMaMa.API.DAL.Entities
{
    public class BusinessDay : SMEntity<int>
    {
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
