using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartUniTest.Helpers;
using SmartUniTest.Models;
using SmartUniTest.Services;

namespace SmartUniTest.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        
        private IEventService _eventService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public EventsController(IEventService eventService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _eventService = eventService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        // GET: api/Events
        [HttpGet]
        public IActionResult GetEvents()
        {
            var ev = _eventService.GetAll();
            return Ok(ev);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public IActionResult GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ev = _eventService.GetById(id);
            return Ok(ev);
        }

        //// PUT: api/Events/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEvent([FromRoute] int id, [FromBody] Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != @event.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(@event).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Events
        [HttpPost]
        public IActionResult PostEvent([FromBody]EventTransferObject eventTransferObject)
        {
            var @event = _mapper.Map<Event>(eventTransferObject);

            try
            {
                // save 
                _eventService.Create(@event);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //// DELETE: api/Events/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var @event = await _context.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Events.Remove(@event);
        //    await _context.SaveChangesAsync();

        //    return Ok(@event);
        //}

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.Id == id);
        //}
    }
}