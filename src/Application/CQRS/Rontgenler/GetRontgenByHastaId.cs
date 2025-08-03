using Application.Dtos.Rontgenler.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Rontgenler
{
	public record GetRontgenByHastaIdQuery(long hastaId) : IRequest<RontgenDto>;

	public class GetRontgenByHastaIdQueryHandler : IRequestHandler<GetRontgenByHastaIdQuery, RontgenDto>
	{
		private readonly IWebDbContext _context;

		public GetRontgenByHastaIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<RontgenDto> Handle(GetRontgenByHastaIdQuery request, CancellationToken cancellationToken)
		{
			var rontgen = await _context.RontgenGoruntuler
				.Include(x => x.Hasta)
				.ThenInclude(h => h.Identity)
				.Where(r => r.HastaId == request.hastaId)
				.OrderByDescending(r => r.CekilmeTarihi) // En son kaydı almak için
				.FirstOrDefaultAsync(cancellationToken);

			if (rontgen == null)
				throw new NotFoundException("Röntgen görüntüsü bulunamadı", nameof(RontgenGoruntu));

			return rontgen.MapToRontgenDto();
		}
	}
}
