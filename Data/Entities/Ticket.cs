using System;
namespace TigerTix.Data.Entities
{
	public class Ticket
	{
		public int id { get; set; } // ticket ID
        public float price {get; set;} // price of ticket
        public bool active { get; set;} // is ticket active?

    }
}