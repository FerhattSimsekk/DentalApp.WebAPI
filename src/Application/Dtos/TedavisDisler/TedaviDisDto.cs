using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.TedavisDisler
{
	public class TedaviDisDto
	{
		public DisKodu Dis { get; set; }
		public List<DisYuzeyTuru> UygulananYuzeyler { get; set; }
		public int TedaviId { get; set; }             // Her dişe farklı tedavi ID'si

	}
}
