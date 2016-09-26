using System;

namespace SugarMaMa.API.DAL.Entities
{
    public class SpaService : SMEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsQuickService { get; set; }
        public bool IsPremium { get; set; }
        public SpaServiceTypes ServiceType { get; set; }
    }

    public enum SpaServiceTypes
    {
        HairRemoval = 0,
        Facial = 1,
        Tanning = 2,
        Tinting = 3,
        ChemicalPeel = 4,
        Microderm = 5,
    }
}
