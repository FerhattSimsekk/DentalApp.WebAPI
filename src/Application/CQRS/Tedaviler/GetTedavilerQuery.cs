using Application.Dtos.Tedavi.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Tedaviler
{
	public record GetTedavilerQuery() : IRequest<List<TedaviDto>>;

	public class GetTedavilerQueryHandler : IRequestHandler<GetTedavilerQuery, List<TedaviDto>>
	{
		private readonly IWebDbContext _context;

		public GetTedavilerQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<List<TedaviDto>> Handle(GetTedavilerQuery request, CancellationToken cancellationToken)
		{
			var tedaviler = await _context.Tedaviler
				.AsNoTracking()
				.OrderBy(t => t.Ad)
				.Select(t => t.MapToTedaviDto())
				.ToListAsync(cancellationToken);

			return tedaviler;
		}
	}
}
