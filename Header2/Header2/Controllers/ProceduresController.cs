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
    public class ProceduresController : ControllerBase
    {
        private readonly LinkingContext _context;

        public ProceduresController(LinkingContext context)
        {
            _context = context;
        }

        //GET: /procedures
        [HttpGet("[controller]")]
        public ActionResult<List<Procedures>> GetProcedures()
        {
            Guid userid = new Guid();

            try
            {
                HttpContext.Request.Headers.TryGetValue("userid", out StringValues value);
                if (value.ToString() == "") return _context.Procedures.ToList();
                userid = Guid.Parse(value.ToString());
            }
            catch
            {
                return BadRequest();
            }

            var procedures = _context.Procedures.ToList().FindAll(opt => opt.UserId == userid);

            if (procedures == null) return BadRequest();
            else return procedures;
        }
    }
}
