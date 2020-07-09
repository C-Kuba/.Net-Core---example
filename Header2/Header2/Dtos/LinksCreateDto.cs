using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Header2.Dtos
{
    public class LinksCreateDto
    {
        public string Resource_type { get; set; }
        public int Resource_id { get; set; }
        public DateTime expires_at { get; set; }


    }
}
