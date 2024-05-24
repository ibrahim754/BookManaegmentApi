using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string ISBN { get; set; }

        public string Title { get; set; }
        public DateTime? PublicationDate { get; set; } = DateTime.Now;
        public string Genre { get; set; }
        [Range(1.0, 5.0)]
        public double Rating { get; set; } = 0;
        public int NumberOfReviews { get; set; } = 0;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Rate> Rates { get; set; }
    }
}
 
