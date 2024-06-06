using TigerTix.Data.Entities;

namespace TigerTix.Data
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(int id);
        void SaveEvent(Event eventData);
        void UpdateEvent(Event eventData);
        void DeleteEvent(Event eventData);
        bool SaveAll();
    }
}