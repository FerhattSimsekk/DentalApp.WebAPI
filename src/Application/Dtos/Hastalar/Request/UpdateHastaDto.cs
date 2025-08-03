using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Hastalar.Request
{
   public class UpdateHastaDto
    {
		public string ad { get; set; } = null!;
		public long tc { get; set; }


		public string soyad { get; set; } = null!;
		public string mail { get; set; } = null!;
	
		public long telefonNo { get; set; }
		public Enums.Gender cinsiyet { get; set; }
		public string adres { get; set; } = null!;
		public string kanGrubu { get; set; }
		public DateTime dogumTarihi { get; set; }
		public Status status { get; set; }
	}
}
