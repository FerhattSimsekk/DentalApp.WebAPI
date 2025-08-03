using Application.Dtos.Odemeler;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Odemeler
{
	public record GetOdemeByIdQuery(int Id) : IRequest<OdemeDetayDto>;

	public class GetOdemeByIdQueryHandler : IRequestHandler<GetOdemeByIdQuery, OdemeDetayDto>
	{
		private readonly IWebDbContext _context;

		public GetOdemeByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<OdemeDetayDto> Handle(GetOdemeByIdQuery request, CancellationToken cancellationToken)
		{
			var odeme = await _context.Odemeler
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (odeme == null)
			{
				throw new Exception("Ödeme bulunamadı.");
			}

			return OdemeMapper.MapToDetayDto(odeme);
		}
	}
}
