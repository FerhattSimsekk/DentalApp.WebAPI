using SampleProjectInterns.Entities;
using static SampleProjectInterns.Entities.Common.Enums;
using Application.Dtos.DisDurumlari;
using Application.Dtos.TedaviKaydiDto;
using Application.Dtos.TedavisDisler;

namespace Application.Mappers
{
	public static class DisDurumuMapper
	{
		public static DisDurumu MapFromDto(DisDurumuDto dto)
		{
			return new DisDurumu
			{
				Id = dto.Id,
				HastaId = dto.HastaId,
				Status=dto.Status,
				DoktorNotu=dto.doktorNotu,
				UygulananDisler = dto.UygulananDisler.Select(dis => new DisDurumDetay
				{
					Dis = dis.Dis,
					Durum = dis.Durum,
					UygulananYuzeyler = dis.Yuzeyler.Select(y => new DisDurumYuzey
					{
						Yuzey = y
					}).ToList()
				}).ToList()
			};
		}
	
		public static DisDurumuDto MapToDto(this DisDurumu entity)
		{
			return new DisDurumuDto
			{
				Id = entity.Id,
				HastaId = entity.HastaId,
				Status=entity.Status,
				tarih=entity.Tarih,
				doktorNotu=entity.DoktorNotu,
				

	

				UygulananDisler = entity.UygulananDisler?.Select(d => new DisDurumDetayDto
				{
					Dis = d.Dis,
					Yuzeyler = d.UygulananYuzeyler?.Select(y => y.Yuzey).ToList(),
					Durum = d.Durum  // Yeni alan map edildi

				}).ToList(),
				adSoyad = entity.Hasta.Identity.Name + " " + entity.Hasta.Identity.LastName,

			};
		}
	}
}
