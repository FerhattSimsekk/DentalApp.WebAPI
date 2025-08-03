using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HastaController : ControllerBase
	{
		private readonly ISender _sender;

		public HastaController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateHastaDto hasta)
		{
			return Ok(await _sender.Send(new CreateHastaCommand(hasta)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, [FromBody] UpdateHastaDto kurye)
		{
			return Ok(await _sender.Send(new UpdateHastaCommand(kurye, id)));
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetHastalarQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetHastaByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteHastaCommand(id)));
		}

	}
}
