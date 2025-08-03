using Application.Dtos.Doktorlar.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class DoktorMapper
	{
		public static DoktorDto MapToDoktorDto(this Doktor doktor)
		{
			return new DoktorDto(
				doktor.Id,
				doktor.Identity.Name,
				doktor.Identity.LastName,
				doktor.Identity.Email,
				doktor.LisansNumarasi,
				doktor.UzmanlikAlani,
				doktor.Status,
				doktor.CreatedAt,
				doktor.UpdatedAt



				);
		}
	}
}
