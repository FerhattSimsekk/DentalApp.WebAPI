using Application.Dtos.Doktorlar.Response;
using Application.Dtos.Hastalar.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Doktors
{
	public record GetDoktorByIdQuery(long DoktorId) : IRequest<DoktorDto>;

	public class GetDoktorByIdQueryHandler : IRequestHandler<GetDoktorByIdQuery, DoktorDto>
	{
		private readonly IWebDbContext _context;

		public GetDoktorByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<DoktorDto> Handle(GetDoktorByIdQuery request, CancellationToken cancellationToken)
		{
			var hasta = await _context.Doktorlar
				.Include(h => h.Identity)
				.AsNoTracking()
				.FirstOrDefaultAsync(h => h.Id == request.DoktorId, cancellationToken)
				?? throw new Exception("Doktor bulunamadı");

			return hasta.MapToDoktorDto();
		}
	}
}
