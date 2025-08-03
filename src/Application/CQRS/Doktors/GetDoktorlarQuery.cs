using Application.Dtos.Doktorlar.Response;
using Application.Dtos.Hastalar.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Doktors
{
	public record GetDoktorlarQuery() : IRequest<List<DoktorDto>>;

	public class GetDoktorlarQueryHandler : IRequestHandler<GetDoktorlarQuery, List<DoktorDto>>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;

		public GetDoktorlarQueryHandler(IWebDbContext context, IPrincipal principal)
		{
			_context = context;
			_principal = principal;
		}

		public async Task<List<DoktorDto>> Handle(GetDoktorlarQuery request, CancellationToken cancellationToken)
		{
		

			// Gerekirse doktorun/kliniğin kimliğini buradan alınabilir
			// var doktorId = identity.DoktorId;

			var hastalar = await _context.Doktorlar
				.Include(h => h.Identity) // Identity bilgilerini alabilmek için
				.AsNoTracking()
				.OrderByDescending(h => h.CreatedAt)
				.Select(h => h.MapToDoktorDto())
				.ToListAsync(cancellationToken);

			return hastalar;
		}
	}
}
