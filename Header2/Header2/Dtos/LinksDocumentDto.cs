using Header2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Header2.Dtos
{

    public class LinksDocumentDto
    {
        public string resource_type { get; set; }
        public DocumentDto resource_content { get; set; }
        
    }
}
