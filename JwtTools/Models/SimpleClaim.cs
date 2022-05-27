using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtTools.Models
{
    public class SimpleClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
