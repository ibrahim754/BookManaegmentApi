using BookManagment.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Models
{
    public class BookOwnerOrAdminRequirement : IAuthorizationRequirement { }

    public class BookOwnerOrAdminHandler : AuthorizationHandler<BookOwnerOrAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseRep<Book> _bookRepository;

        public BookOwnerOrAdminHandler(IHttpContextAccessor httpContextAccessor, IBaseRep<Book> bookRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _bookRepository = bookRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BookOwnerOrAdminRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                context.Fail();
                return;
            }

            var isAdmin = context.User.IsInRole("Admin");
            if (isAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            var routeData = _httpContextAccessor.HttpContext.GetRouteData();
            var bookIdStr = routeData.Values["id"]?.ToString();
            if (bookIdStr == null || !int.TryParse(bookIdStr, out int bookId))
            {
                context.Fail();
                return;
            }

            var book =   _bookRepository.GetById(bookId); // Await the async method call
            if (book == null || book.UserId != userId)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }
    }
}
