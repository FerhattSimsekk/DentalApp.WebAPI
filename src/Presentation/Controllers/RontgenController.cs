using Application.CQRS.DisDurumlari;
using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.Rontgenler;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using Application.Dtos.Rontgenler.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RontgenController : ControllerBase
	{
		private readonly ISender _sender;

		public RontgenController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] CreateRontgenDto hasta)
		{
			return Ok(await _sender.Send(new CreateRontgenCommand(hasta)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromForm] UpdateRontgenDto kurye)
		{
			return Ok(await _sender.Send(new UpdateRontgenCommand(id, kurye)));
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetRontgenlerQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await _sender.Send(new GetRontgenByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok(await _sender.Send(new DeleteRontgenCommand(id)));
		}
		[HttpGet("hasta/{hastaId}")]
		public async Task<IActionResult> GetByHastaId(long hastaId)
		{
			return Ok(await _sender.Send(new GetRontgenByHastaIdQuery(hastaId)));
		}

	}
}
