using Application.Dtos.Hastalar.Response;
using Application.Dtos.Rontgenler.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iTextSharp.text.pdf.events.IndexEvents;

namespace Application.Mappers
{
   public static class RontgenMapper
    {
		public static RontgenDto MapToRontgenDto(this RontgenGoruntu rontgen)
		{
			return new RontgenDto(
			rontgen.Id,
			rontgen.HastaId,
			rontgen.Hasta != null ? rontgen.Hasta.Identity.Name + " " + rontgen.Hasta.Identity.LastName : "",
			rontgen.GoruntuYolu,
			rontgen.Status



				);
		}
	}
}
