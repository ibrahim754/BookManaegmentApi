using BookManagment.Core.Dtos;
using BookManagment.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Interfaces
{
    public interface IBaseRep<T> where T : class
    {
        public GetBookDto GetByIdDto(int id);
        public T GetById(int id);

        public T GetById(String id);

        public IEnumerable<GetBookDto> GetAll();
        public T GetFind(Expression<Func<T, bool>> match);
        public IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        public T GetFindIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        public T? Add(T entity);
        public T? Update (T entity);
        public T? Delete (T entity);
        string RateFeedBack(int bookId, RateDto rateDto);
        public IEnumerable<GetBookDto> SearchByAuthororTitle(string UserName);
        public IQueryable<T> Serach(Expression<Func<T, bool>> predicate);
        public List<T> Filter(Expression<Func<T, bool>> predicate);
        public List<Book> Sorting(string sortOption);
        public List<Tuple<string, int>> ElementsPerCategory();
        public List<RatePerBookDto> RatePerBookDtos();
        public List<GetBookDto> OrderBooksByRateThenReviews();
            

    }

}
