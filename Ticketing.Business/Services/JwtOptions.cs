using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Business.Services
{
    public class JwtOptions
    {
        public string Issure { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
