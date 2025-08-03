using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Tedavi.Request
{
    public class TedaviCreateDto
    {

		public string Ad { get; set; } = null!;
		public string Aciklama { get; set; }
		public decimal Ucret { get; set; }
		
	}
}
