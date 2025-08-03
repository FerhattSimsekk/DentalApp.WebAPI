using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.Rontgenler;
using Application.CQRS.Tedaviler;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using Application.Dtos.Tedavi.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TedaviController : ControllerBase
	{
		private readonly ISender _sender;

		public TedaviController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TedaviCreateDto hasta)
		{
			return Ok(await _sender.Send(new CreateTedaviCommand(hasta)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] TedaviUpdateDto kurye)
		{
			return Ok(await _sender.Send(new UpdateTedaviCommand(kurye, id)));
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetTedavilerQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await _sender.Send(new GetTedaviByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok(await _sender.Send(new DeleteTedaviCommand(id)));
		}

	}
}
