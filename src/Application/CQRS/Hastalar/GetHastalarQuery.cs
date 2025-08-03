using Application.Dtos.Hastalar.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Hastalar
{
	public record GetHastalarQuery() : IRequest<List<HastaDto>>;

	public class GetHastalarQueryHandler : IRequestHandler<GetHastalarQuery, List<HastaDto>>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;

		public GetHastalarQueryHandler(IWebDbContext context, IPrincipal principal)
		{
			_context = context;
			_principal = principal;
		}

		public async Task<List<HastaDto>> Handle(GetHastalarQuery request, CancellationToken cancellationToken)
		{
		

			// Gerekirse doktorun/kliniğin kimliğini buradan alınabilir
			// var doktorId = identity.DoktorId;

			var hastalar = await _context.Hastalar
				.Include(h => h.Identity) // Identity bilgilerini alabilmek için
				.AsNoTracking()
				.OrderByDescending(h => h.CreatedAt)
				.Select(h => h.MapToHastaDto())
				.ToListAsync(cancellationToken);

			return hastalar;
		}
	}
}
