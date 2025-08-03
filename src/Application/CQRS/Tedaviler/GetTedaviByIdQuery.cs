using Application.Dtos.Tedavi.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Exceptions;

namespace Application.CQRS.Tedaviler
{
	public record GetTedaviByIdQuery(int Id) : IRequest<TedaviDto>;

	public class GetTedaviByIdQueryHandler : IRequestHandler<GetTedaviByIdQuery, TedaviDto>
	{
		private readonly IWebDbContext _context;

		public GetTedaviByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<TedaviDto> Handle(GetTedaviByIdQuery request, CancellationToken cancellationToken)
		{
			var tedavi = await _context.Tedaviler
				.AsNoTracking()
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
				?? throw new NotFoundException("Tedavi bulunamadı", "tedavi");

			return tedavi.MapToTedaviDto();
		}
	}
}
