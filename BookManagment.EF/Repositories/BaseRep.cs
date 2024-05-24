using BookManagment.Core.Dtos;
using BookManagment.Core.Interfaces;
using BookManagment.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.EF.Repositories
{
    public class BaseRep<T> : IBaseRep<T> where T : class
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _context;
        public BaseRep(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public T ?Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while saving:");
                Console.WriteLine(ex.ToString()); // Output the full exception details for debugging
                return null;
            }
            return entity;
        }

        public T? Delete(T entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<GetBookDto> GetAll()
        {
             return _context.Books
                    
                        
                          .Select(book => new GetBookDto
                          {
                              UserId = book.UserId,
                              Id = book.Id,
                              Genre = book.Genre,
                              ISBN = book.ISBN,
                              Rating = book.Rating,
                              Title = book.Title,
                              UserName = book.User.UserName,
                              PublicationDate = book.PublicationDate,
                              NumberOfReviews = book.NumberOfReviews
                          })
                          .ToList();
        }

        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.ToList();
        }

        public GetBookDto GetByIdDto(int id)
        {
            var book = _context.Books.Include(b => b.User).FirstOrDefault(e => e.Id == id);
            GetBookDto dto = new GetBookDto() { Id = id , Genre = book.Genre, ISBN = book.ISBN, NumberOfReviews = book.NumberOfReviews, PublicationDate =book.PublicationDate,
                Rating = book.Rating, Title = book.Title,UserId = book.UserId,UserName = book.User.UserName, 

            };
            return dto;
        }

        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetFind(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().FirstOrDefault(match);
        }

        public T GetFindIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.FirstOrDefault(match);
        }

        public T? Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChanges();
                return entity;
            }catch (Exception ex)
            {
                return null;

            }
        }
        public string RateFeedBack(int bookId, RateDto rateDto)
        {
            var book = _context.Books.Find(bookId);

            var rate = new Rate { BookId = bookId, Rating = rateDto.Rating, Review = rateDto.Review, UserId = rateDto.UserId };

            try
            {
                _context.Rates.Add(rate);
                _context.SaveChanges();

                // you can this get from the equation rating = (TotakReviewValues / numberofRev);
                double totalReviewsValue = (book.Rating * book.NumberOfReviews);
                totalReviewsValue += rate.Rating;
                book.NumberOfReviews++;
                book.Rating = totalReviewsValue / book.NumberOfReviews;
                _context.SaveChanges();

                 return "updated Succfully";
            }
            catch (Exception ex) { return ex.InnerException?.Message ?? ex.Message; }

        }

        public IEnumerable<GetBookDto> SearchByAuthororTitle(string UserNameOrTitle)
        {
            return _context.Books
                           .Include(b => b.User)
                           .Where(book => book.User.UserName.Contains(UserNameOrTitle) || book.Title.Contains(UserNameOrTitle))
                           .Select(book => new GetBookDto
                           {
                               UserId = book.UserId,
                               Id = book.Id,
                               Genre = book.Genre,
                               ISBN = book.ISBN,
                               Rating = book.Rating,
                               Title = book.Title,
                               UserName = book.User.UserName,
                               PublicationDate = book.PublicationDate,
                               NumberOfReviews = book.NumberOfReviews
                           });
        }
        public IQueryable<T> Serach(Expression<Func<T, bool>> predicate)
        {

            var searchList = _context.Set<T>().Where(predicate);
            return searchList;
        }
        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            var filteredList = _context.Set<T>().Where(predicate).ToList();
            return filteredList;
        }

        public List<Book> Sorting(string sortOption)
        {
            List<Book> filters = new List<Book>();
            var books = _context.Books.Include(b  => b.User).ToList();
            switch (sortOption.ToLower())
            {
                case "title":
                    filters = books.OrderBy(e => e.Title).ToList();
                    break;
                case "author":
                    filters = books.OrderBy(e => e.User.UserName).ToList();
                    break;
                case "publication date":
                    filters = books.OrderBy(e => e.PublicationDate).ToList();
                    break;
                case "rating":
                    filters = books.OrderBy(e => e.Rating).ToList();
                    break;

            }
             return filters;
        }

        public List<Tuple<string, int>> ElementsPerCategory()
        {
            return _context.Set<Book>()
                  .GroupBy(e => e.Genre)  
                  .Select(group => Tuple.Create(group.Key, group.Count()))  
                  .ToList<Tuple<string, int>>();

        }
        public List<RatePerBookDto> RatePerBookDtos()
        {
            return _context.Books
                           .Select(book => new RatePerBookDto
                           {
                               Title = book.Title,
                               AverageRate = book.Rating,
                               TotalNumberOfReviews = book.NumberOfReviews
                           })
                           .ToList();
        }
        public List<GetBookDto> OrderBooksByRateThenReviews()
        {
            return _context.Books
                           .OrderByDescending(book => book.Rating)
                           .ThenByDescending(book => book.NumberOfReviews)
                           .Select(book => new GetBookDto
                           {
                               UserId = book.UserId,
                               Id = book.Id,
                               Genre = book.Genre,
                               ISBN = book.ISBN,
                               Rating = book.Rating,
                               Title = book.Title,
                               UserName = book.User.UserName,
                               PublicationDate = book.PublicationDate,
                               NumberOfReviews = book.NumberOfReviews
                           })
                           .ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
