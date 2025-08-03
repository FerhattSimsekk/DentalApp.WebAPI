using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Rontgenler.Request
{
	public class UpdateRontgenDto
	{
		public long hastaId { get; set; }
		public IFormFile? goruntuYolu { get; set; }
		public Status status { get; set; }
	}
}
