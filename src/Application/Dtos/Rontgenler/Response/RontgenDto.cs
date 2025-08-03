using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Rontgenler.Response
{
	public record RontgenDto(
	  long id,
	  long hastaId,
	  string hastaAdSoyad,

	  string? goruntuYolu,
	  Status status
  );
}
