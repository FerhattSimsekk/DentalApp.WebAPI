using Application.Dtos.Hastalar.Response;
using Application.Dtos.Tedavi.Response;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
	public static class TedaviMapper
	{
		public static TedaviDto MapToTedaviDto(this Tedavi tedavi)
		{
			return new TedaviDto(
				tedavi.Id,
				tedavi.Ad,
				tedavi.Aciklama,
				tedavi.Ucret,
				tedavi.Status
				
				



				);
		}
	}
}
