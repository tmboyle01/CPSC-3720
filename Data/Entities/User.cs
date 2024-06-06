﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace TigerTix.Data.Entities
{
	public class User
	{
		public int Id {  get; set; }
		public string UserName { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password{get;set;}
		public bool  isAdmin {get;set;}
		public byte[] userImage {get;set;}


        //public ICollection<Ticket> Tickets { get; }


		

        


    }
}

