﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcurementService.DbModels;

namespace GrpcShared.Models
{
	public class TeamRestaurantsItems
	{
		public int ID { get; set; }
		public int TeamRestaurantsID { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }

		//TODO: add currency type to DB

		public int CreatedById { get; set; }
		public DateTime CreatedOn { get; set; }
		public int UpdatedById { get; set; }
		public DateTime UpdatedOn { get; set; }

		public TeamRestaurants TeamRestaurants { get; set; }
		public Persons CreatedBy { get; set; }
		public Persons UpdatedBy { get; set; }
	}
}
