using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.DisDurumlari
{
	public class DisDurumuDto
	{
		public int Id { get; set; }
		public long HastaId { get; set; }
		public string adSoyad { get; set; }
		public List<DisDurumDetayDto> UygulananDisler { get; set; }
		public Status Status { get; set; }
		public DateTime tarih { get; set; }
		public string doktorNotu { get; set; }
	}
	public class DisDurumDetayDto
	{
		public DisKodu Dis { get; set; }
		public DisDurumTuru Durum { get; set; }
		public List<DisYuzeyTuru> Yuzeyler { get; set; }
	}


}
