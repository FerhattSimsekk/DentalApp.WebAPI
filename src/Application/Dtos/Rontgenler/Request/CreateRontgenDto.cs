using Microsoft.AspNetCore.Http;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Rontgenler.Request
{
   public class CreateRontgenDto
    {
		public long hastaId { get; set; }
		public IFormFile goruntuYolu { get; set; } // Dosya yolu veya blob url

		
	}
}
