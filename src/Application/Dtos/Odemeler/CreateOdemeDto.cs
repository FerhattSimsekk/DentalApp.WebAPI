using System;
using System.ComponentModel.DataAnnotations;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Odemeler
{
	public class CreateOdemeDto
	{
		[Required]
		public int TedaviKaydiId { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "Tutar sıfırdan büyük olmalıdır")]
		public decimal Tutar { get; set; }

		public DateTime Tarih { get; set; } = DateTime.UtcNow;

	
		public OdemeTipi OdemeTipi { get; set; }
		
	}
}
