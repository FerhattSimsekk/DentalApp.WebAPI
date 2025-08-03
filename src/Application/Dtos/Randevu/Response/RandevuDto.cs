using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Randevu.Response
{
	public class RandevuDto
	{
		public int id { get; set; }
		public long hastaId { get; set; }
		public string hastaAdSoyad { get; set; }
		public long doktorId { get; set; }
		public string doktorAdSoyad { get; set; }
		public DateTime baslangicTarihi { get; set; }
		public DateTime bitisTarihi { get; set; }
		public Status status { get; set; }
		public string aciklama { get; set; }
	}
}
