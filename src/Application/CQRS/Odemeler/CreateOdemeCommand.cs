using Application.Dtos.Odemeler;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Threading;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Odemeler
{
	public record CreateOdemeCommand(CreateOdemeDto OdemeDto) : IRequest<int>;

	public class CreateOdemeCommandHandler : IRequestHandler<CreateOdemeCommand, int>
	{
		private readonly IWebDbContext _context;

		public CreateOdemeCommandHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<int> Handle(CreateOdemeCommand request, CancellationToken cancellationToken)
		{
			var dto = request.OdemeDto;

			var tedaviKaydi = await _context.TedaviKayitlari
				.Include(t => t.Odemeler)
				.FirstOrDefaultAsync(t => t.Id == dto.TedaviKaydiId, cancellationToken);

			

			var odeme = new Odeme
			{
				TedaviKaydiId = dto.TedaviKaydiId,
				Tutar = dto.Tutar,
				OdemeTarihi = dto.Tarih.ToUniversalTime(),
				status=Status.approved
			};

			tedaviKaydi.Odemeler.Add(odeme);

			// Yeni toplam ödeme
			var odenenToplam = tedaviKaydi.Odemeler.Sum(o => o.Tutar);
			var kalan = tedaviKaydi.ToplamUcret - odenenToplam;

			tedaviKaydi.OdemeDurumu =
				kalan <= 0 ? OdemeDurumu.Odenmis :
				odenenToplam > 0 ? OdemeDurumu.KismiOdenmis :
				OdemeDurumu.Odenmedi;

			await _context.SaveChangesAsync(cancellationToken);

			return odeme.Id;
		}
	}
}
