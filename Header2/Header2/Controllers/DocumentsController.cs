using Header2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Header2.Controllers
{
    [Route("")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly LinkingContext _context;

        public DocumentsController(LinkingContext context)
        {
            _context = context;
        }

        //GET: /documents
        [HttpGet("[controller]")]
        public ActionResult<List<Documents>> GetDocuments()
        {
            Guid userid = new Guid();

            try
            {
                HttpContext.Request.Headers.TryGetValue("userid", out StringValues value);
                if (value.ToString() == "") return _context.Documents.ToList();
                userid = Guid.Parse(value.ToString());
            }
            catch
            {
                return BadRequest();
            }

            var documents = _context.Documents.ToList().FindAll(opt => opt.UserId == userid);

            if (documents == null) return BadRequest();
            else return documents;
        }
    }
}
