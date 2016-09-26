using System;

namespace SugarMaMa.API.DAL.Entities
{
    public class SpaService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsQuickService { get; set; }
        public bool IsPremium { get; set; }
    }

    public enum SpaServiceTypes
    {
        HairRemoval,
        Facial,
        Tanning,
        Tinting,
        ChemicalPeel,
        Microderm,
    }
}
