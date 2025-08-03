using Application.Dtos.TedavisDisler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.TedaviKaydiDto
{
	public class TedaviKaydiCreateDto
	{
		public long HastaId { get; set; }
		public DateTime Tarih { get; set; }
		public TedaviDurumu Durum { get; set; }
		


		public List<TedaviDisDto> UygulananDisler { get; set; }
	}


}
