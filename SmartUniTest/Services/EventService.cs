using SmartUniTest.Helpers;
using SmartUniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartUniTest.Services
{
    public class EventService : IEventService
    {
        private DataContext _context;
        public EventService(DataContext context)
        {
            _context = context;
            _context.SaveChanges();
        }

        public Event Create(Event @event)
        {
            if (string.IsNullOrEmpty(@event.Date) || string.IsNullOrEmpty(@event.Description))
                throw new AppException("Date and Description are required");

            _context.Events.Add(@event);
            _context.SaveChanges();

            return @event;
        }

        public void Delete(int id)
        {
            var ev = _context.Events.Find(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                _context.SaveChanges();
            };
        }

        public IQueryable GetAll()
        {
            return from Event in _context.Events orderby Event.Date select new { Event };
        }

        public IQueryable GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable GetEventsById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
