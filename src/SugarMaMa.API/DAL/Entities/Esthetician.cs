using System;
using System.Collections.Generic;

namespace SugarMaMa.API.DAL.Entities
{
    public class Esthetician : SMEntity<int>
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<SpaService> Services { get; set; }
    }
}
