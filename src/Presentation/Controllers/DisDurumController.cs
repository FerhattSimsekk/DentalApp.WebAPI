using Application.CQRS.DisDurumlari;
using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.TedaviKayitlari;
using Application.CQRS.Tedaviler;
using Application.Dtos.DisDurumlari;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Tedavi.Request;
using Application.Dtos.TedaviKaydiDto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisDurumController : ControllerBase
    {
		private readonly ISender _sender;

		public DisDurumController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]DisDurumuCreateDto tedaviKaydi)
		{
			return Ok(await _sender.Send(new CreateDisDurumuCommand(tedaviKaydi)));
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] DisDurumuCreateDto kurye)
		{
			return Ok(await _sender.Send(new UpdateDisDurumuCommand(id, kurye)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetAllDisDurumlariQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await _sender.Send(new GetDisDurumuByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok(await _sender.Send(new DeleteDisDurumuCommand(id)));
		}
		[HttpGet("hasta/{hastaId}")]
		public async Task<IActionResult> GetByHastaId(long hastaId)
		{
			return Ok(await _sender.Send(new GetDisDurumlariByHastaIdQuery(hastaId)));
		}

	}
}
