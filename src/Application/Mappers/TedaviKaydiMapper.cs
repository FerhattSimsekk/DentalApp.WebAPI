using Application.Dtos.TedaviKaydiDto;
using Application.Dtos.TedavisDisler;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Mappers
{
	public static class TedaviKaydiMapper
	{
		public static TedaviKaydi MapFromDto(TedaviKaydiCreateDto dto)
		{
			var entity = new TedaviKaydi
			{
				HastaId = dto.HastaId,
				Tarih = dto.Tarih,
				Durum = dto.Durum,
				UygulananDisler = dto.UygulananDisler.Select(disDto => new TedaviDis
				{
					Dis = disDto.Dis,
					TedaviId = disDto.TedaviId,           // Yeni alanı map et
					UygulananYuzeyler = disDto.UygulananYuzeyler.Select(yuzeyDto => new TedaviDisYuzeyi
					{
						Yuzey = yuzeyDto
					}).ToList()

				}).ToList()
			};

			return entity;
		}
		public static TedaviKaydiDto MapToDto(this TedaviKaydi entity)
		{
			return new TedaviKaydiDto
			{
				Id = entity.Id,
				HastaId = entity.HastaId,
				DoktorId = entity.DoktorId,
				
				Tarih = entity.Tarih,
				Durum = entity.Durum,
				ToplamUcret = entity.ToplamUcret,
				OdemeDurumu = entity.OdemeDurumu, // ✅ Bu satır önemli

				UygulananDisler = entity.UygulananDisler?.Select(d => new TedaviDisDto
				{
					Dis = d.Dis,
					UygulananYuzeyler = d.UygulananYuzeyler?.Select(y => y.Yuzey).ToList(),
					TedaviId = d.TedaviId  // Yeni alan map edildi

				}).ToList(),
				adSoyad=entity.Hasta.Identity.Name+" "+entity.Hasta.Identity.LastName,

			};
		}

	}
}
