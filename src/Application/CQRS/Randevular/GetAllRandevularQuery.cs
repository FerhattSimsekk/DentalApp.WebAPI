using Application.Dtos.Randevu.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Randevular
{
	public record GetAllRandevularQuery() : IRequest<List<RandevuDto>>;

	public class GetAllRandevularQueryHandler : IRequestHandler<GetAllRandevularQuery, List<RandevuDto>>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;

		public GetAllRandevularQueryHandler(IWebDbContext context, IPrincipal principal)
		{
			_context = context;
			_principal = principal;
		}

		public async Task<List<RandevuDto>> Handle(GetAllRandevularQuery request, CancellationToken cancellationToken)
		{
			var identity = await _context.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("Kullanıcı bulunamadı");

			var doktor = await _context.Doktorlar
				.FirstOrDefaultAsync(d => d.IdentityId == identity.Id, cancellationToken)
				?? throw new Exception("Doktor bulunamadı");

			var randevular = await _context.Randevular
				.Include(r => r.Hasta).ThenInclude(h => h.Identity)
				.Include(r => r.Doktor).ThenInclude(d => d.Identity)
				.Where(r => r.DoktorId == doktor.Id&& r.Durum!=SampleProjectInterns.Entities.Common.Enums.Status.deleted )
				.OrderByDescending(r => r.BaslangicTarihi)
				.ToListAsync(cancellationToken);

			return randevular.Select(RandevuMapper.ToDto).ToList();

		}
	}
}
