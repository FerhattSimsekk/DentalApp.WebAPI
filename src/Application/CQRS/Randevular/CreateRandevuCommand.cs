using Application.Dtos.Randevu.Request;
using Application.Dtos.Randevu.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using SampleProjectInterns.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Randevular
{
	public record CreateRandevuCommand(CreateRandevuDto Randevu) : IRequest<int>;

	public class CreateRandevuCommandHandler : IRequestHandler<CreateRandevuCommand, int>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public CreateRandevuCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<int> Handle(CreateRandevuCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");
			var doktor = await _webDbContext.Doktorlar.AsNoTracking().FirstOrDefaultAsync(z => z.IdentityId == identity.Id);
			var entity = new Randevu
			{
				HastaId = request.Randevu.hastaId,
				DoktorId = doktor.Id,
				BaslangicTarihi = request.Randevu.baslangicTarihi.ToUniversalTime(),
				BitisTarihi = request.Randevu.bitisTarihi.ToUniversalTime(),
				Durum = SampleProjectInterns.Entities.Common.Enums.Status.approved,
				Aciklama=request.Randevu.aciklama
			};

			_webDbContext.Randevular.Add(entity);
			await _webDbContext.SaveChangesAsync(cancellationToken);

			// Include Hasta ve Doktor için eager loading yapılması önerilir
			var randevuWithIncludes = await _webDbContext.Randevular
				.Include(r => r.Hasta)
				.ThenInclude(x=>x.Identity)
				.Include(r => r.Doktor)
				.ThenInclude(y=>y.Identity)
				.FirstOrDefaultAsync(r => r.Id == entity.Id, cancellationToken);

			return entity.Id;
		}
	}
}
