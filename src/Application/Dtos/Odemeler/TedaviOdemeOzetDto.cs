using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Odemeler
{
	public class TedaviOdemeOzetDto
	{
		public string HastaAdSoyad { get; set; } = default!;
		public DateTime Tarih { get; set; }
		public decimal ToplamUcret { get; set; }
		public decimal Odenen { get; set; }
		public decimal Kalan => ToplamUcret - Odenen;
		public OdemeDurumu Durum { get; set; } 
		public List<OdemeDetayDto> Odemeler { get; set; } = new();
	}

	public class OdemeDetayDto
	{
		public int Id { get; set; }

		public DateTime Tarih { get; set; }
		public decimal Tutar { get; set; }
		public Status status { get; set; }
	}
}
