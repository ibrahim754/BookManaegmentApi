using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Models
{
    public class Rate
    {
      
        public int BookId { get; set; }
        public Book Book { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Range(1.0, 5.0)]
        public double Rating { get; set; }
        [MaxLength(277)]
        public string Review { get; set; }

    }

}
