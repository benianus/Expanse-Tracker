using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpanseTrackerDataLayer
{
    public class MonthBudgetDto
    {
        public int MonthId { get; set; }
        public decimal? Budget { get; set; }
        public MonthBudgetDto(int monthId, decimal? budget)
        { 
            MonthId = monthId;
            Budget = budget;
        }
    }
}
