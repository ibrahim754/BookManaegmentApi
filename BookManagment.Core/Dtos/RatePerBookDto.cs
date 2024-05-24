using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Dtos
{
    public class RatePerBookDto
    {
        public string Title { get; set; }
        public double AverageRate { get; set; }
        public int TotalNumberOfReviews { get; set; }
    }
}
