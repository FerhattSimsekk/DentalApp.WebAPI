using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Doktorlar.Request
{
   public class CreateDoktorDto
    {
		public string ad { get; set; } = null!;
		public string soyad { get; set; } = null!;
		public string mail { get; set; } = null!;
		public string password { get; set; } = null!;
		public string lisansNumarasi { get; set; } = null!;
		public string uzmanlikAlani { get; set; } = null!;

	}
}
