using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Odemeler
{
	public class UpdateOdemeDto
	{
		public decimal Tutar { get; set; }
		public DateTime Tarih { get; set; }
		public string? Aciklama { get; set; }
		public Status status { get; set; }
	}

}
