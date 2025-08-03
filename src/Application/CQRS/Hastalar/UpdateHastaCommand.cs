using Application.Dtos.Hastalar.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Hastalar
{
	public record UpdateHastaCommand(UpdateHastaDto hasta, long hastaId) : IRequest;

	public class UpdateHastaCommandHandler : IRequestHandler<UpdateHastaCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public UpdateHastaCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<Unit> Handle(UpdateHastaCommand request, CancellationToken cancellationToken)
		{
			
			var hasta = await _webDbContext.Hastalar
				.Include(h => h.Identity) // Identity bilgilerini güncellemek için Include gerekir
				.FirstOrDefaultAsync(h => h.Id == request.hastaId, cancellationToken)
				?? throw new NotFoundException($"{request.hasta.ad} not found", "hasta");

			// Identity üzerinden ad, soyad, mail güncelle
			hasta.Identity.Name = request.hasta.ad;
			hasta.Identity.LastName = request.hasta.soyad;
			hasta.Identity.Email = request.hasta.mail;

			// Hasta üzerinden diğer bilgileri güncelle
			hasta.TelefonNo = request.hasta.telefonNo;
			hasta.Tc = request.hasta.tc;
			hasta.Adres = request.hasta.adres;
			hasta.KanGrubu = request.hasta.kanGrubu;
			hasta.DogumTarihi = request.hasta.dogumTarihi.ToUniversalTime();
			hasta.Status = request.hasta.status;
			hasta.UpdatedAt = DateTime.UtcNow;
			hasta.Cinsiyet = request.hasta.cinsiyet;

			await _webDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
