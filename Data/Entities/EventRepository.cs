using Microsoft.EntityFrameworkCore;
using TigerTix.Data.Entities;

namespace TigerTix.Data
{
    public class EventRepository : IEventRepository
    {
        private readonly TigerTixContext _context;

        public EventRepository(TigerTixContext context)
        {
            _context = context;
        }

        // Save event to DB
        public void SaveEvent(Event eventData)
        {
            if(eventData.EventImage==null){
                 byte[] temp = {Convert.ToByte(0)};
                eventData.EventImage = temp;
            }
            _context.Add(eventData);
            _context.SaveChanges();
        } 


        // Returns all events from DB
        public IEnumerable<Event> GetAllEvents()
        {
            var eventList = from e in _context.Events
                            select e;
            return eventList.ToList();
        }

        // Return a single event based on unique ID
        public Event GetEventById(int id)
        {
            return _context.Events.FirstOrDefault(e => e.Id == id);
        }

        // Update event
        public void UpdateEvent(Event eventData)
        {    
            _context.Events.Update(eventData);
            _context.SaveChanges();
        }

        // Delete event
        public void DeleteEvent(Event eventData)
        {
            _context.Events.Remove(eventData);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}