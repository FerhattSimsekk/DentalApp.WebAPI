using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Hastalar.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Application.CQRS.Doktors
{
	public record UpdateDoktorCommand(UpdateDoktorDto hasta, long doktorId) : IRequest;

	public class UpdateDoktorCommandHandler : IRequestHandler<UpdateDoktorCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public UpdateDoktorCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<Unit> Handle(UpdateDoktorCommand request, CancellationToken cancellationToken)
		{
			
			var hasta = await _webDbContext.Doktorlar
				.Include(h => h.Identity) // Identity bilgilerini güncellemek için Include gerekir
				.FirstOrDefaultAsync(h => h.Id == request.doktorId, cancellationToken)
				?? throw new NotFoundException($"{request.hasta.ad} not found", "hasta");

			// Identity üzerinden ad, soyad, mail güncelle
			hasta.Identity.Name = request.hasta.ad;
			hasta.Identity.LastName = request.hasta.soyad;
			hasta.Identity.Email = request.hasta.mail;

			// Hasta üzerinden diğer bilgileri güncelle
			hasta.Status = request.hasta.status;
			hasta.UpdatedAt = DateTime.UtcNow;
			hasta.UzmanlikAlani = request.hasta.uzmanlikAlani;
			hasta.LisansNumarasi = request.hasta.lisansNumarasi;

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
