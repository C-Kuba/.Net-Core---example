using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Header2.Models
{
    public class Links
    {
        [Key]
        public string id { get; set; }
        public string resource_type { get; set; }
        public int resource_id { get; set; }
        public string password { get; set; }

    }
}
