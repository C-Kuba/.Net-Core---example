using Header2.Dtos;
using Header2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Header2.Controllers
{
    [Route("")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly LinkingContext _context;

        public LinksController(LinkingContext context)
        {
            _context = context;
        }

        //GET: /Links
        [HttpGet("[controller]")]
        public ActionResult<List<Links>> GetLinks()
        {
            Guid userid = new Guid();

            try
            {
                HttpContext.Request.Headers.TryGetValue("userid", out StringValues value);
                if (value.ToString() == "") return _context.Links.ToList();
                userid = Guid.Parse(value.ToString());
            }
            catch
            {
                return BadRequest();
            }

            var links = _context.Links.ToList();
            List<Links> link = new List<Links>();

            List<Documents> documents = _context.Documents.ToList();
            List<Procedures> procedures = _context.Procedures.ToList();

            foreach (var procedure in procedures)
            {
                if (procedure.UserId == userid)
                {
                    Links item = links.Find(opt => opt.resource_id == procedure.id && opt.resource_type == "procedure");
                    link.Add(item);
                }
            }

            foreach (var document in documents)
            {
                if (document.UserId == userid)
                {
                    Links item = links.Find(opt => opt.resource_id == document.id && opt.resource_type == "document");
                    link.Add(item);
                }
            }

            return Ok(link);
        }

        // GET links/5
        [HttpGet("[controller]/{id}")]
        public ActionResult GetLink(string id, string pass)
        {
            var links = _context.Links.ToList().Find(opt => opt.id == id);

            if (links.password != pass) return Unauthorized();

            if (links == null) return BadRequest();

            if (links.resource_type == "procedure")
            {
                var procedure = _context.Procedures.ToList().Find(opt => opt.id == links.resource_id);

                var proc = new ProceduresDto()
                {
                    Name = procedure.Name,
                    Date = procedure.Date
                };

                var link = new LinksProcedureDto()
                {
                    resource_type = links.resource_type,
                    resource_content = proc
                };
                return Ok(link);
            }
            else if (links.resource_type == "document")
            {
                var document = _context.Documents.ToList().Find(opt => opt.id == links.resource_id);

                var doc = new DocumentDto()
                {
                    Title = document.Title,
                    Content = document.Content
                };

                var link = new LinksDocumentDto()
                {
                    resource_type = links.resource_type,
                    resource_content = doc
                };
                return Ok(link);
            }
            else return BadRequest();
        }


        //POST: /links
        [HttpPost("[controller]")]
        public ActionResult <LinksCreateDto> PostContent(LinksCreateDto item)
        {
            if (item.expires_at.CompareTo(DateTime.Now) <= 0) return Unauthorized();

            Guid userid = new Guid();
            Links link = new Links();


            try
            {
                HttpContext.Request.Headers.TryGetValue("userid", out StringValues value);
                userid = Guid.Parse(value.ToString());
            }
            catch
            {
                return BadRequest();
            }

            link.resource_id = item.Resource_id;
            link.resource_type = item.Resource_type;
            link.password = GetUniqueKey(8);

            if (link.resource_type == "procedure")
            {
                var procedure = _context.Procedures.ToList().Find(opt => opt.id == link.resource_id);
                if (procedure == null) return BadRequest();

                link.id = GetUniqueKey(8);
                _context.Links.Add(link);
                _context.SaveChanges();
                return CreatedAtAction("GetLink", new { id = link.id, pass = link.password }, link);
            }
            else if (link.resource_type == "document")
            {
                var document = _context.Documents.ToList().Find(opt => opt.id == link.resource_id);
                if (document == null) return BadRequest();

                link.id = GetUniqueKey(8);
                _context.Links.Add(link);
                _context.SaveChanges();
                return CreatedAtAction("GetLink", new { id = link.id }, link);
            }
            else return BadRequest();
        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[36];
            chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);
            data = new byte[maxSize];
            crypto.GetBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


    }
}
