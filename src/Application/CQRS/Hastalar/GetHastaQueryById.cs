using Application.Dtos.Hastalar.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Hastalar
{
	public record GetHastaByIdQuery(long HastaId) : IRequest<HastaDto>;

	public class GetHastaByIdQueryHandler : IRequestHandler<GetHastaByIdQuery, HastaDto>
	{
		private readonly IWebDbContext _context;

		public GetHastaByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<HastaDto> Handle(GetHastaByIdQuery request, CancellationToken cancellationToken)
		{
			var hasta = await _context.Hastalar
				.Include(h => h.Identity)
				.AsNoTracking()
				.FirstOrDefaultAsync(h => h.Id == request.HastaId, cancellationToken)
				?? throw new Exception("Hasta bulunamadı");

			return hasta.MapToHastaDto();
		}
	}
}
