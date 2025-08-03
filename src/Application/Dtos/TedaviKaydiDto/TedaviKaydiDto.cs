using Application.Dtos.TedavisDisler;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.TedaviKaydiDto
{
      public class TedaviKaydiDto
    {
		public int Id { get; set; }
		public long HastaId { get; set; }
		public long tc { get; set; }
		public long DoktorId { get; set; }
		public DateTime Tarih { get; set; }
		public TedaviDurumu Durum { get; set; }
		public decimal ToplamUcret { get; set; }  // Tedavi ücretini buraya alacağız
		public OdemeDurumu OdemeDurumu { get; set; } = OdemeDurumu.Odenmedi;


		public List<TedaviDisDto> UygulananDisler { get; set; }

		public string adSoyad { get; set; }

	}

  
}
