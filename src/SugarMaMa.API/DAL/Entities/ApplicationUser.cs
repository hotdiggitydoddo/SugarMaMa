using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SugarMaMa.API.DAL.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
    }
}
