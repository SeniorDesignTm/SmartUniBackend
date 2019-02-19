using SmartUniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartUniTest.Services
{
    public interface IEventService
    {

        IQueryable GetAll();
        IQueryable GetById(int id);
        Event Create(Event @event);
        void UpdateEvent(Event @event);
        IQueryable GetEventsById(int id);
        void Delete(int id);
    }
}
