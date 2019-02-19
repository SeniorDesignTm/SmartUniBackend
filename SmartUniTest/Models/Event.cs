using System.Collections.Generic;

namespace SmartUniTest.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        //public IList<UserEvent> UserEvents { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
    }
}