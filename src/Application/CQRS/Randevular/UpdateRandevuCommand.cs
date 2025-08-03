using Application.Dtos.Randevu.Request;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;

namespace Application.CQRS.Randevular
{
	public record UpdateRandevuCommand(int Id, UpdateRandevuDto Randevu) : IRequest<int>;

	public class UpdateRandevuCommandHandler : IRequestHandler<UpdateRandevuCommand, int>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public UpdateRandevuCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<int> Handle(UpdateRandevuCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User not found");

			var doktor = await _webDbContext.Doktorlar.AsNoTracking()
				.FirstOrDefaultAsync(d => d.IdentityId == identity.Id, cancellationToken)
				?? throw new Exception("Doctor not found");

			var entity = await _webDbContext.Randevular
				.FirstOrDefaultAsync(r => r.Id == request.Id && r.DoktorId == doktor.Id, cancellationToken)
				?? throw new Exception("Randevu not found or unauthorized");

			entity.HastaId = request.Randevu.hastaId;
			entity.BaslangicTarihi = request.Randevu.baslangicTarihi.ToUniversalTime();
			entity.BitisTarihi = request.Randevu.bitisTarihi.ToUniversalTime();
			entity.Durum = request.Randevu.status;
			entity.Aciklama = request.Randevu.aciklama;

			await _webDbContext.SaveChangesAsync(cancellationToken);

			return entity.Id;
		}
	}
}
