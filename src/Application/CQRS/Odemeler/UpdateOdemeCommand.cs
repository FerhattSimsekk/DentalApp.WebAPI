using Application.Dtos.Odemeler;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Odemeler
{
	public record UpdateOdemeCommand(int Id, UpdateOdemeDto OdemeDto) : IRequest<int>;

	public class UpdateOdemeCommandHandler : IRequestHandler<UpdateOdemeCommand, int>
	{
		private readonly IWebDbContext _context;

		public UpdateOdemeCommandHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<int> Handle(UpdateOdemeCommand request, CancellationToken cancellationToken)
		{
			var odeme = await _context.Odemeler
				.Include(x => x.TedaviKaydi)
				.ThenInclude(t => t.Odemeler)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (odeme == null)
				throw new Exception("Ödeme bulunamadı");

			// Güncelleme
			odeme.Tutar = request.OdemeDto.Tutar;
			odeme.OdemeTarihi = request.OdemeDto.Tarih.ToUniversalTime();
			odeme.status = request.OdemeDto.status;

			// Ödeme durumu güncelle
			var tedaviKaydi = odeme.TedaviKaydi;
			var toplamOdenen = tedaviKaydi.Odemeler.Where(x => x.Id != odeme.Id).Sum(x => x.Tutar) + odeme.Tutar;
			var kalan = tedaviKaydi.ToplamUcret - toplamOdenen;

			tedaviKaydi.OdemeDurumu =
				kalan <= 0 ? OdemeDurumu.Odenmis :
				toplamOdenen > 0 ? OdemeDurumu.KismiOdenmis :
				OdemeDurumu.Odenmedi;

			await _context.SaveChangesAsync(cancellationToken);

			return odeme.Id;
		}
	}
}
