using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Dtos
{
    public class RateDto
    {
        public string UserId { get; set; }

        [Range(1.0, 5.0)]
        public double Rating { get; set; }
        [MaxLength(277)]
        public string Review { get; set; }
    }
    public class GetRateDto
    {

    }
}
