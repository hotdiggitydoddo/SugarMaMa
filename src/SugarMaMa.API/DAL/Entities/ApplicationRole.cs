using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid();
        }
        public ApplicationRole(string name)
           : this()
        {
            this.Name = name;
        }
    }
}
