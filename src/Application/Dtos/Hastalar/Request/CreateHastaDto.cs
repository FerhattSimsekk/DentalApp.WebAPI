using SampleProjectInterns.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Hastalar.Request
{
   public class CreateHastaDto
    {
		public long tc { get; set; }
		public string ad { get; set; } = null!;
		public string soyad { get; set; } = null!;
		public string mail { get; set; } = null!;
		public DateTime dogumTarihi { get; set; }
		public long telefonNo { get; set; }
		public Enums.Gender? cinsiyet { get; set; }
		public string adres { get; set; } = null!;
		public string kanGrubu { get; set; }

	}
}
