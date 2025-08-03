using Application.Dtos.Doktorlar.Response;
using Application.Dtos.Hastalar.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class HastaMapper
	{
		public static HastaDto MapToHastaDto(this Hasta hasta)
		{
			return new HastaDto(
				hasta.Id,
				hasta.Tc,
				hasta.Identity.Name,
				hasta.Identity.LastName,
				hasta.Identity.Email,
				hasta.TelefonNo,
				hasta.DogumTarihi,
				hasta.Adres,
				hasta.KanGrubu,
				hasta.Cinsiyet,
				hasta.Status,
				hasta.CreatedAt,
				hasta.UpdatedAt



				);
		}
	}
}
