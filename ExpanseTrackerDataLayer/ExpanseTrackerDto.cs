using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseTrackerDataLayer
{
    public class ExpanseTrackerDto
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public ExpanseTrackerDto(int? id, DateTime? date, string? description, decimal? amount) 
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
        }
        
    }
}
