using Header2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Header2.Dtos
{
    public class LinksProcedureDto
    {
        public string resource_type { get; set; }
        public ProceduresDto resource_content { get; set; }
    }
}
