using Application.Dtos.DisDurumlari;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.DisDurumlari;

public record GetAllDisDurumlariQuery() : IRequest<List<DisDurumuDto>>;

public class GetAllDisDurumlariQueryHandler : IRequestHandler<GetAllDisDurumlariQuery, List<DisDurumuDto>>
{
	private readonly IWebDbContext _webDbContext;

	public GetAllDisDurumlariQueryHandler(IWebDbContext webDbContext)
	{
		_webDbContext = webDbContext;
	}

	public async Task<List<DisDurumuDto>> Handle(GetAllDisDurumlariQuery request, CancellationToken cancellationToken)
	{
		var disDurumlari = await _webDbContext.DisDurumlari
			.Include(x => x.Hasta).ThenInclude(h => h.Identity)
			.Include(x => x.UygulananDisler)
				.ThenInclude(d => d.UygulananYuzeyler)
			.ToListAsync(cancellationToken);

		return disDurumlari.Select(d => d.MapToDto()).ToList();
	}
}
