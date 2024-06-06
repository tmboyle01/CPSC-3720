using System;
namespace TigerTix.Data.Entities
{
	public class Event
	{
		public int Id {  get; set; }
		public string EventName { get; set; }
        public string EventTime { get; set; }
		public string EventDescription { get; set; }

        public string EventAddress { get; set; }

         public string EventCost { get; set; }

		 public byte[] EventImage { get; set; }


   





        

    }
}

