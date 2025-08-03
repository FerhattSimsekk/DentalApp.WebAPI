using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Odemeler
{
	public class OdemeListItemDto
	{
		public int Id { get; set; }
		public string HastaAdSoyad { get; set; }
		public decimal Tutar { get; set; }
		public DateTime Tarih { get; set; }
		public int TedaviKaydiId { get; set; }
		public Status status { get; set; }

	}
}
