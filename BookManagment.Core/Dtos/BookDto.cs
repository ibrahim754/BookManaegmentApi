using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Dtos
{
    public class UpdateBookDto   
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
    }
    public class BookDto:UpdateBookDto
    {
        public string UserId { get; set; }


    }
    public class GetBookDto:BookDto
    {
        public int Id { get; set; }
        public Double? Rating { get; set; }
        public String UserName { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int NumberOfReviews {  get; set; }

    }
    
}
