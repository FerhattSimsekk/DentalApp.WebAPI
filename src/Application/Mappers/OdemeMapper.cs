using Application.Dtos.Odemeler;
using SampleProjectInterns.Entities;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Mappers
{
	public static class OdemeMapper
	{
		public static TedaviOdemeOzetDto MapToOdemeOzet(TedaviKaydi tedavi)
		{
			var odenen = tedavi.Odemeler?.Sum(x => x.Tutar) ?? 0;

			return new TedaviOdemeOzetDto
			{

				HastaAdSoyad = tedavi.Hasta.Identity.Name + " " + tedavi.Hasta.Identity.LastName,
				Tarih = tedavi.Tarih,
				ToplamUcret = tedavi.ToplamUcret,
				Odenen = odenen,
				Durum = odenen == 0
					? OdemeDurumu.Odenmedi
					: odenen >= tedavi.ToplamUcret
						? OdemeDurumu.Odenmis
						: OdemeDurumu.KismiOdenmis,
				Odemeler = tedavi.Odemeler.Select(o => new OdemeDetayDto
				{
					Id = o.Id,
					Tarih = o.OdemeTarihi,
					Tutar = o.Tutar,
				}).ToList()
			};
		}
		public static OdemeListItemDto MapToListItemDto(Odeme odeme)
		{
			return new OdemeListItemDto
			{
				Id = odeme.Id,
				Tarih = odeme.OdemeTarihi,
				Tutar = odeme.Tutar,
				TedaviKaydiId = odeme.TedaviKaydiId,
				HastaAdSoyad = odeme.TedaviKaydi.Hasta.Identity.Name + " " + odeme.TedaviKaydi.Hasta.Identity.LastName,
				status=odeme.status
				
			};
		}
		public static OdemeDetayDto MapToDetayDto(Odeme odeme)
		{
			return new OdemeDetayDto
			{
				Id = odeme.Id,
				Tarih = odeme.OdemeTarihi,
				Tutar = odeme.Tutar,
				status=odeme.status
			
			};
		}

	}
}
