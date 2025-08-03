using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.Odemeler;
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using Application.Dtos.Odemeler;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OdemeController : ControllerBase
	{
		private readonly ISender _sender;

		public OdemeController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateOdemeDto odeme)
		{
			return Ok(await _sender.Send(new CreateOdemeCommand(odeme)));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] UpdateOdemeDto odeme)
		{
			return Ok(await _sender.Send(new UpdateOdemeCommand(id, odeme)));
		}

		[HttpGet]
		public async Task<IActionResult> Get(int tedaviId)
		{
			return Ok(await _sender.Send(new GetTedaviOdemeOzetQuery(tedaviId)));
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _sender.Send(new GetAllOdemelerQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(await _sender.Send(new GetOdemeByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteHastaCommand(id)));
		}

	}
}
