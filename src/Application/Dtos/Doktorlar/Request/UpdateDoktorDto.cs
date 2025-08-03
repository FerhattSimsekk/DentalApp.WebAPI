using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Doktorlar.Request
{
   public class UpdateDoktorDto
    {
		public string ad { get; set; } = null!;
		public string soyad { get; set; } = null!;
		public string mail { get; set; } = null!;
		public string lisansNumarasi { get; set; } = null!;
		public string uzmanlikAlani { get; set; } = null!;
		public Status status { get; set; }
	}
}
