using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalonCRM.Models
{
    public class HostProfile
    {
        public int ProfileID { get; set; }
        public int HostID { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyText { get; set; }
    }
}