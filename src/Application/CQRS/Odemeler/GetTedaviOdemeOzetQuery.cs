using Application.Dtos.Odemeler;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Odemeler
{
	public record GetTedaviOdemeOzetQuery(int TedaviKaydiId) : IRequest<TedaviOdemeOzetDto>;

	public class GetTedaviOdemeOzetQueryHandler : IRequestHandler<GetTedaviOdemeOzetQuery, TedaviOdemeOzetDto>
	{
		private readonly IWebDbContext _context;

		public GetTedaviOdemeOzetQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<TedaviOdemeOzetDto> Handle(GetTedaviOdemeOzetQuery request, CancellationToken cancellationToken)
		{
			var tedavi = await _context.TedaviKayitlari
				.Include(x => x.Hasta)
					.ThenInclude(h => h.Identity)
				.Include(x => x.Odemeler)
				.FirstOrDefaultAsync(x => x.Id == request.TedaviKaydiId, cancellationToken);

			if (tedavi == null)
				throw new Exception("Tedavi kaydı bulunamadı.");

			return OdemeMapper.MapToOdemeOzet(tedavi);
		}
	}
}
