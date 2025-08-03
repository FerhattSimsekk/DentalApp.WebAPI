using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.Dtos.Hastalar.Response
{
	public record HastaDto(
long id,
long tc,
string ad,
string soyad,
string mail,
long telefonNo,
DateTime dogumTarihi,
string adres,
string kanGrubu,
Gender cinsiyet,
Status status,
DateTime created,
DateTime? updated

);
}
