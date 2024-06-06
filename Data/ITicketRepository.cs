using TigerTix.Data.Entities;

namespace TigerTix.Data
{
    public interface ITicketRepository
    {        
        void CreateTicket(Ticket ticket);        
        void DeleteTicket(Ticket ticket);
        Ticket GetTicketId(int id);
        IEnumerable<Ticket> GetAllTickets();
        bool SaveAll();

    }

}