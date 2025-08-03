using Application.Dtos.TedaviKaydiDto;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.TedaviKayitlari
{
	public record GetTedaviKaydiByIdQuery(int Id) : IRequest<TedaviKaydiDto>;

	public class GetTedaviKaydiByIdQueryHandler : IRequestHandler<GetTedaviKaydiByIdQuery, TedaviKaydiDto>
	{
		private readonly IWebDbContext _context;

		public GetTedaviKaydiByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<TedaviKaydiDto> Handle(GetTedaviKaydiByIdQuery request, CancellationToken cancellationToken)
		{
			var kayit = await _context.TedaviKayitlari
				.Include(k => k.UygulananDisler)
					.ThenInclude(d => d.UygulananYuzeyler)
					.Include(x=>x.Hasta)
					.ThenInclude(y=>y.Identity)
				.FirstOrDefaultAsync(k => k.Id == request.Id, cancellationToken);

			if (kayit == null)
				throw new NotFoundException("Tedavi kaydı bulunamadı", "tedavi kaydı");

			return kayit.MapToDto();
		}
	}
}
