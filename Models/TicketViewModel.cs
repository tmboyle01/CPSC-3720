using System;
namespace TigerTix.Modelspublic 
{
    class TicketViewModel
    {   
        public int id { get; set; } // ticket ID
        public int price {get; set;} // price of ticket
        public bool active { get; set;} // is ticket active?

    }   
}
