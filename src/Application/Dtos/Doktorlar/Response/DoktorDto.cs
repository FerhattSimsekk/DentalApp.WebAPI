using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Doktorlar.Response
{
	public record DoktorDto(
long id,
string ad,
string soyad,
string mail,
string lisansNumarasi,
string uzmanlikAlani,
Status status,
DateTime created,
DateTime? updated

);
}
