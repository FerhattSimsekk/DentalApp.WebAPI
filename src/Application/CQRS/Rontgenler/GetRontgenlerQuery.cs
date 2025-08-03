using Application.Dtos.Rontgenler.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Rontgenler
{
	public record GetRontgenlerQuery() : IRequest<List<RontgenDto>>;

	public class GetRontgenlerQueryHandler : IRequestHandler<GetRontgenlerQuery, List<RontgenDto>>
	{
		private readonly IWebDbContext _context;

		public GetRontgenlerQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<List<RontgenDto>> Handle(GetRontgenlerQuery request, CancellationToken cancellationToken)
		{
			var rontgenler = await _context.RontgenGoruntuler
				.Include(x => x.Hasta)
				.ThenInclude(h => h.Identity)
				.ToListAsync(cancellationToken);

			return rontgenler.Select(x => x.MapToRontgenDto()).ToList();
		}
	}
}
