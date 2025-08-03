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
	public record GetRontgenByIdQuery(int Id) : IRequest<RontgenDto>;

	public class GetRontgenByIdQueryHandler : IRequestHandler<GetRontgenByIdQuery, RontgenDto>
	{
		private readonly IWebDbContext _context;

		public GetRontgenByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<RontgenDto> Handle(GetRontgenByIdQuery request, CancellationToken cancellationToken)
		{
			var rontgen = await _context.RontgenGoruntuler
				.Include(x => x.Hasta)
				.ThenInclude(h => h.Identity)
				.FirstOrDefaultAsync(r => r.Id == request.Id , cancellationToken);

			if (rontgen == null)
				throw new NotFoundException("Röntgen görüntüsü bulunamadı", nameof(RontgenGoruntu));

			return rontgen.MapToRontgenDto();
		}
	}
}
