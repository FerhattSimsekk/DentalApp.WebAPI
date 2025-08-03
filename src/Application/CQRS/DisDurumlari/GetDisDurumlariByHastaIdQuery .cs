using Application.Dtos.DisDurumlari;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.DisDurumlari;

public record GetDisDurumlariByHastaIdQuery(long HastaId) : IRequest<List<DisDurumuDto>>;

public class GetDisDurumlariByHastaIdQueryHandler : IRequestHandler<GetDisDurumlariByHastaIdQuery, List<DisDurumuDto>>
{
	private readonly IWebDbContext _webDbContext;

	public GetDisDurumlariByHastaIdQueryHandler(IWebDbContext webDbContext)
	{
		_webDbContext = webDbContext;
	}

	public async Task<List<DisDurumuDto>> Handle(GetDisDurumlariByHastaIdQuery request, CancellationToken cancellationToken)
	{
		var disDurumlari = await _webDbContext.DisDurumlari
			.Include(x => x.Hasta).ThenInclude(h => h.Identity)
			.Include(x => x.UygulananDisler)
				.ThenInclude(d => d.UygulananYuzeyler)
			.Where(d => d.HastaId == request.HastaId)
			.ToListAsync(cancellationToken);

		return disDurumlari.Select(d => d.MapToDto()).ToList();
	}
}
