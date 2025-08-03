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
    public class DoktorController : ControllerBase
    {
		private readonly ISender _sender;

		public DoktorController(ISender sender)
		{
			_sender = sender;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody]CreateDoktorDto doktor)
		{
			return Ok(await _sender.Send(new CreateDoktorCommand(doktor)));
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(long id, [FromBody] UpdateDoktorDto kurye)
		{
			return Ok(await _sender.Send(new UpdateDoktorCommand(kurye, id)));
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _sender.Send(new GetDoktorlarQuery()));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(long id)
		{
			return Ok(await _sender.Send(new GetDoktorByIdQuery(id)));
		}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(long id)
		{
			return Ok(await _sender.Send(new DeleteDoktorCommand(id)));
		}
	}
}
