using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloteer
{
    public class UserProfile
    {
        public string? Username { get; set; }
        public string? Profile { get; set; }
        public bool MFA { get; set; }
    }
}
