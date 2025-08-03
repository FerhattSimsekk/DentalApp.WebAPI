using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Tedavi.Response
{
	public record TedaviDto(
long id,
string ad,
string aciklama,
decimal ucret,
Status status


);
}
