using Books.Service.Core.Entites;
using Books.Service.Core.Enums;
using Books.Service.Core.Interfaces;
using Books.Service.Core.Helpers;
using Books.Service.Core.Requests;
using MediatR;

namespace Books.Service.Core.Handlers;

public class GetBooksHandler : IRequestHandler<GetBooksRequest, IEnumerable<Book>>
{
    private readonly IBookRepository _bookRepository;
 
    public GetBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<IEnumerable<Book>> Handle(GetBooksRequest request, CancellationToken cancellationToken)
    {
        var orderByExpression = OrderByOptions()[request.SortBy].Expression;
        var books = _bookRepository.GetBooks().ToList().AsQueryable();

        IEnumerable<Book> orderedResults = Queryable.OrderBy(books, orderByExpression);

        return Task.FromResult(orderedResults);
    }

    private static Dictionary<SortBy, IBookOrderBy> OrderByOptions()
        => new() 
        {
            { SortBy.Title, new BookOrderBy<string>(x => x.Title) },
            { SortBy.Author, new BookOrderBy<string>(x => x.Author) },
            { SortBy.Price, new BookOrderBy<decimal>(x => x.Price) }
        };
}