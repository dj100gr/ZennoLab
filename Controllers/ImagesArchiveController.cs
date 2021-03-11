using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZennoLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesArchiveController : ControllerBase
    {
        // GET: api/ImagesArchive
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ImagesArchive/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ImagesArchive
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ImagesArchive/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
