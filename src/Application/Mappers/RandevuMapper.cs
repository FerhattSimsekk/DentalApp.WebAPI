using Application.Dtos.Randevu.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class RandevuMapper
	{
		public static RandevuDto ToDto(Randevu entity)
		{
			return new RandevuDto
			{
				id = entity.Id,
				hastaId = entity.HastaId,
				hastaAdSoyad = entity.Hasta != null ? entity.Hasta.Identity.Name + " " + entity.Hasta.Identity.LastName : "",
				doktorId = entity.DoktorId,
				doktorAdSoyad = entity.Doktor != null ? entity.Doktor.Identity.Name + " " + entity.Doktor.Identity.LastName : "",
				baslangicTarihi = entity.BaslangicTarihi,
				bitisTarihi = entity.BitisTarihi,
				status = entity.Durum,
				aciklama = entity.Aciklama
			};
		}
	}
}
