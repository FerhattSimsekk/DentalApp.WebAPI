using Application.CQRS.Doktors;
using Application.CQRS.Hastalar;
using Application.CQRS.TedaviKayitlari;
using Application.CQRS.Tedaviler;
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
    public class TedaviKaydiController : ControllerBase
    {
		private readonly ISender _sender;

		public TedaviKaydiController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]TedaviKaydiCreateDto tedaviKaydi)
		{
			return Ok(await _sender.Send(new CreateTedaviKaydiCommand(tedaviKaydi)));
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] TedaviKaydiCreateDto kurye)
		{
			return Ok(await _sender.Send(new UpdateTedaviKaydiCommand(id, kurye)));
		}
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetTedaviKayitlariQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			return Ok(await _sender.Send(new GetTedaviKaydiByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok(await _sender.Send(new DeleteTedaviKaydiCommand(id)));
		}

	}
}
