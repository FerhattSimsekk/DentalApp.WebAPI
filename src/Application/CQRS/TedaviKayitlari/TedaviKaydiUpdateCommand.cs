using Application.Dtos.TedaviKaydiDto;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using static SampleProjectInterns.Entities.Common.Enums;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Principal;

namespace Application.CQRS.TedaviKayitlari
{
	public record UpdateTedaviKaydiCommand(int Id, TedaviKaydiCreateDto TedaviKaydi) : IRequest<int>;

	public class UpdateTedaviKaydiCommandHandler : IRequestHandler<UpdateTedaviKaydiCommand, int>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public UpdateTedaviKaydiCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<int> Handle(UpdateTedaviKaydiCommand request, CancellationToken cancellationToken)
		{
			// Kayıt var mı kontrol et (Include ile ilişkili koleksiyonları getir)
			var entity = await _webDbContext.TedaviKayitlari
				.Include(t => t.UygulananDisler)
					.ThenInclude(d => d.UygulananYuzeyler)
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

			
				

			// Doktor bilgisi al (mevcut kullanıcıya göre)
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");

			var doktor = await _webDbContext.Doktorlar.AsNoTracking()
				.FirstOrDefaultAsync(d => d.IdentityId == identity.Id, cancellationToken);

			if (doktor == null)
				throw new Exception("Doktor bilgisi bulunamadı");

			var dto = request.TedaviKaydi;

			// Alanları güncelle
			entity.HastaId = dto.HastaId;
			entity.Tarih = dto.Tarih.ToUniversalTime();
			entity.Durum = dto.Durum;
			entity.DoktorId = doktor.Id;
		

			// Toplam ücret sıfırla
			decimal toplamUcret = 0;

			// UygulananDisler güncellemesi
			// 1. Mevcut dişleri listele
			var mevcutDisler = entity.UygulananDisler.ToList();

			// 2. Silinecekleri kaldır
			foreach (var mevcutDis in mevcutDisler)
			{
				if (!dto.UygulananDisler.Any(d => d.TedaviId == mevcutDis.TedaviId && d.Dis == mevcutDis.Dis))
				{
					_webDbContext.TedaviDisler.Remove(mevcutDis);
				}
			}

			// 3. Yeni ve güncel dişler için işlemler
			foreach (var disDto in dto.UygulananDisler)
			{
				var mevcutDis = entity.UygulananDisler
					.FirstOrDefault(d => d.TedaviId == disDto.TedaviId && d.Dis == disDto.Dis);

				if (mevcutDis == null)
				{
					// Yeni diş ekle
					var yeniDis = new TedaviDis
					{
						TedaviId = disDto.TedaviId,
						Dis = disDto.Dis,
						UygulananYuzeyler = disDto.UygulananYuzeyler
							.Select(y => new TedaviDisYuzeyi { Yuzey = y })
							.ToList()
					};
					entity.UygulananDisler.Add(yeniDis);

					// Ücret hesaplama için
					var tedaviUcret = await _webDbContext.Tedaviler
						.Where(t => t.Id == disDto.TedaviId)
						.Select(t => t.Ucret)
						.FirstOrDefaultAsync(cancellationToken);

					toplamUcret += tedaviUcret;
				}
				else
				{
					// Var olan dişi güncelle
					// Önce yüzeyleri temizle
					mevcutDis.UygulananYuzeyler.Clear();
					foreach (var yuzey in disDto.UygulananYuzeyler)
					{
						mevcutDis.UygulananYuzeyler.Add(new TedaviDisYuzeyi { Yuzey = yuzey });
					}

					// Ücret hesaplama için
					var tedaviUcret = await _webDbContext.Tedaviler
						.Where(t => t.Id == disDto.TedaviId)
						.Select(t => t.Ucret)
						.FirstOrDefaultAsync(cancellationToken);

					toplamUcret += tedaviUcret;
				}
			}

			entity.ToplamUcret = toplamUcret;

			// Değişiklikleri kaydet
			await _webDbContext.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
