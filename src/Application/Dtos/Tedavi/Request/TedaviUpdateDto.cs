using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Tedavi.Request
{
     public class TedaviUpdateDto
    {
		public string Ad { get; set; } = null!;
		public string Aciklama { get; set; }
		public decimal Ucret { get; set; }
		public Status status { get; set; }
	} 
}
