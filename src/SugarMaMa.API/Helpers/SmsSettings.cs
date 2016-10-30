using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace SugarMaMa.API.Helpers
{
    public class SmsSettings
    {
        public string Sid { get; set; }
        public string Token { get; set; }
        public string BaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
    }
}
