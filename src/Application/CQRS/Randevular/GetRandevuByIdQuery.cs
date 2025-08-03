using Application.Dtos.Randevu.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;

namespace Application.CQRS.Randevular
{
	public record GetRandevuByIdQuery(int Id) : IRequest<RandevuDto>;

	public class GetRandevuByIdQueryHandler : IRequestHandler<GetRandevuByIdQuery, RandevuDto>
	{
		private readonly IWebDbContext _context;

		public GetRandevuByIdQueryHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<RandevuDto> Handle(GetRandevuByIdQuery request, CancellationToken cancellationToken)
		{
			var randevu = await _context.Randevular
				.Include(r => r.Hasta).ThenInclude(h => h.Identity)
				.Include(r => r.Doktor).ThenInclude(d => d.Identity)
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

			if (randevu == null)
				throw new NotFoundException("Randevu bulunamadı", "randevu");

			return RandevuMapper.ToDto(randevu);
		}
	}
}
