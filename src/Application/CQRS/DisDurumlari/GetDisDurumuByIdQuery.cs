using Application.Dtos.DisDurumlari;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.DisDurumlari
{
	public record GetDisDurumuByIdQuery(int Id) : IRequest<DisDurumuDto>;

	public class GetDisDurumuByIdQueryHandler : IRequestHandler<GetDisDurumuByIdQuery, DisDurumuDto>
	{
		private readonly IWebDbContext _context;

		public GetDisDurumuByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<DisDurumuDto> Handle(GetDisDurumuByIdQuery request, CancellationToken cancellationToken)
		{
			var entity = await _context.DisDurumlari
				.Include(d => d.Hasta).ThenInclude(h => h.Identity)
				.Include(d => d.UygulananDisler)
					.ThenInclude(ud => ud.UygulananYuzeyler)
				.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

			if (entity == null)
				throw new KeyNotFoundException("Dis durumu bulunamadı");

			return DisDurumuMapper.MapToDto(entity);
		}
	}
}
