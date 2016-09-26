using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SugarMaMa.API.DAL.Entities
{
    public class SMEntity<T>
    {
        public T Id { get; set; }
    }
}
