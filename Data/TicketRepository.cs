using Microsoft.EntityFrameworkCore;
using System;
using TigerTix.Data.Entities;

namespace TigerTix.Data
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TigerTixContext _context;

        public TicketRepository(TigerTixContext context)
        {
            _context = context;
        }

        // Save ticket to database
        public void CreateTicket(Ticket ticket) 
        {
            _context.Add(ticket);
            _context.SaveChanges();
        }     
        
        public void DeleteTicket(Ticket ticket) 
        {
            _context.Remove(ticket);
            _context.SaveChanges();
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            var tList = from t in _context.Tickets
                           select t;
            return tList.ToList();
        }

        public Ticket GetTicketId(int ticket)
        {
            return _context.Tickets.FirstOrDefault(t => t.id == ticket);
        }

        public Ticket GetTicketPrice(float ticket)
        {
            return _context.Tickets.FirstOrDefault(t => t.price == ticket);
        }

        // to use, create a separate bool variable | ex. bool isActive = IsTicketActive(ticket)
        public bool IsTicketActive(Ticket ticket) 
        {
            return ticket != null && ticket.active;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}