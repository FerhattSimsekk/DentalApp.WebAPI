using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.Randevular;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using Application.Dtos.Randevu.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RandevuController : ControllerBase
	{
		private readonly ISender _sender;

		public RandevuController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateRandevuDto hasta)
		{
			return Ok(await _sender.Send(new CreateRandevuCommand(hasta)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] UpdateRandevuDto kurye)
		{
			return Ok(await _sender.Send(new UpdateRandevuCommand(id, kurye)));
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetAllRandevularQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await _sender.Send(new GetRandevuByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok(await _sender.Send(new DeleteRandevuCommand(id)));
		}

	}
}
