using BookManagment.Core.Dtos;
using BookManagment.Core.Interfaces;
using BookManagment.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookManagmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public class BookController : ControllerBase
    {
        private readonly IBaseRep<Book> _books;
        private readonly IBaseRep<Rate> _rates;
        private readonly IAuthService _authService;

        public BookController(IBaseRep<Book> books, IBaseRep<Rate> rates, IAuthService authService)
        {
            _books = books;
            _rates = rates;
            _authService = authService;
        }

        // CRUD Operation 
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(_books.GetAll());
        }

        [HttpGet("{id}")]

        public IActionResult GetBook(int id)
        {
            return Ok(_books.GetByIdDto(id));
        }


        [HttpPost]
        public async Task<IActionResult> PostBook(BookDto bookDto)
        {
            Book book = new Book
            {
                Title = bookDto.Title,
                UserId = bookDto.UserId,
                Genre = bookDto.Genre,
                ISBN = bookDto.ISBN
            };
            var res = _books.Add(book);
            ApplicationUser user = await _authService.GetUserByIdAsync(book.UserId);
            return res == null ? BadRequest("Failed to be Added ") : Ok(_books.GetByIdDto(res.Id));
                
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto bookDto)
        {

            var book = _books.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            if (!IsAuthonicated(book.UserId)) return Unauthorized();

            book.Title = bookDto.Title;
            book.Genre = bookDto.Genre;
            book.ISBN = bookDto.ISBN;
            _books.Update(book);
            var roless = await _authService.GetRolesAsync(book.UserId);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _books.GetById(id);
            if (book == null) return NotFound();
            bool isAuthonicated = IsAuthonicated(book.UserId);
            if (!isAuthonicated) return Unauthorized();
            _books.Delete(book);
            return Ok();

        }
        // UserFeedBack
        [HttpPost("FeedBack/{id}")]
        public IActionResult BookFeedBack(int id, RateDto rateDto)
        {
            var book = _books.GetById(id);
            if (IsAuthonicated(book.UserId)) return BadRequest("You can not Leave Comment Either you are Admin or the Pupliser");
            var ret = _books.RateFeedBack(id, rateDto);
            if (ret != "updated Succfully") return BadRequest(ret);
            return Ok();

        }
        // Apply Search, if any book's Title or pupliser matches
        [HttpPost("SearchByAuthorOrTitle")]
        public IActionResult SearchByAuthororTitle(string UserNameOrAuthor)
        {
            var books = _books.SearchByAuthororTitle(UserNameOrAuthor);
            return Ok(books);
        }
        // Apply Filter by Genere, Publication Date Range(Min,Max), and Average Rate ;
        [HttpPost("FilterByGenre")]
        public IActionResult FilterByGenre(string Genere)
        {
            var filteredList = _books.Filter(b => b.Genre.ToLower() == Genere.ToLower());
            return Ok(filteredList);

        }
        [HttpPost("FilterBypublicationDateRange")]
        public IActionResult FilterBypublicationDateRange(DateTime minTime, DateTime maxTime)
        {
            var filteredList = _books.Filter(b => b.PublicationDate >= minTime && b.PublicationDate <= maxTime);
            return Ok(filteredList);

        }
        [HttpPost("FilterByAverageRate")]
        public IActionResult FilterByAverageRate(double averageRate)
        {

            var filteredList = _books.Filter(b => b.Rating == averageRate);
            return Ok(filteredList);

        }

        // [Bonus Points (Plus) ] Book Statistics: 
        [HttpGet("NumberOfBookInGenere")]
        public IActionResult Genere()
        {
            var generes = _books.ElementsPerCategory();
          

            return Ok( generes);
        }
        [HttpGet("RatingPerBook")]
        public IActionResult RatingPerBook()
        {
            return Ok(_books.RatePerBookDtos());
        }

        [HttpGet("OrderBooksByRateThenReviews")]
        public IActionResult OrderBooksByRateThenReviews()
        {
            return Ok(_books.OrderBooksByRateThenReviews());
        }

        // Sorting endpoint, Generic one, 
        [HttpGet("sort")]
        public IActionResult SortBooks(string sortOptions)
        {
            var sortedList = _books.Sorting(sortOptions);
            if (sortedList.IsNullOrEmpty()) return BadRequest();
            return Ok(sortedList);
        }

        private bool IsAuthonicated(string UserUid)
        {
            var userUid = User.FindFirstValue("uid");
            if (userUid == null) return false;
            if (userUid == UserUid) return true;
            var userRoles = User.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            if (userRoles == null) return false;
            if (userRoles.Contains("Admin")) return true;
            return false;

        }
    }
}
