using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Randevu.Request
{
    public class UpdateRandevuDto
    {
		public long hastaId { get; set; }
		public DateTime baslangicTarihi { get; set; }
		public DateTime bitisTarihi { get; set; }
		public Status status { get; set; }
		public string aciklama { get; set; }
	}
}
