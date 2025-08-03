using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Odemeler
{
	public class OdemeDto
	{
		public decimal Tutar { get; set; }
		public DateTime OdemeTarihi { get; set; }
		public OdemeTipi OdemeTipi { get; set; }
		public Status status { get; set; }
	}
}
