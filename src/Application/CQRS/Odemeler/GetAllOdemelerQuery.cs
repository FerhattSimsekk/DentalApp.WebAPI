using Application.Dtos.Odemeler;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Odemeler
{
	public record GetAllOdemelerQuery() : IRequest<List<OdemeListItemDto>>;

	public class GetAllOdemelerQueryHandler : IRequestHandler<GetAllOdemelerQuery, List<OdemeListItemDto>>
	{
		private readonly IWebDbContext _context;

		public GetAllOdemelerQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<List<OdemeListItemDto>> Handle(GetAllOdemelerQuery request, CancellationToken cancellationToken)
		{
			var odemeler = await _context.Odemeler
				.Include(o => o.TedaviKaydi)
					.ThenInclude(t => t.Hasta)
						.ThenInclude(h => h.Identity)
				.AsNoTracking()
				.ToListAsync(cancellationToken);

			return odemeler
				.Select(OdemeMapper.MapToListItemDto)
				.ToList();
		}
	}
}
