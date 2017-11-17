using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/ListsApi")]
    public class ListsApiController : Controller
    {
        private readonly ToDoDbContext _context;

        public ListsApiController(ToDoDbContext context)
        {
            _context = context;
        }

        // GET: api/ListsApi
        [HttpGet]
        public IEnumerable<List> GetList()
        {
            return _context.List;
        }

        // GET: api/ListsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        // PUT: api/ListsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutList([FromRoute] int id, [FromBody] List list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.ListId)
            {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ListsApi
        [HttpPost]
        public async Task<IActionResult> PostList([FromBody] List list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListId }, list);
        }

        // DELETE: api/ListsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = await _context.List.SingleOrDefaultAsync(m => m.ListId == id);
            if (list == null)
            {
                return NotFound();
            }

            _context.List.Remove(list);
            await _context.SaveChangesAsync();

            return Ok(list);
        }

        private bool ListExists(int id)
        {
            return _context.List.Any(e => e.ListId == id);
        }
    }
}