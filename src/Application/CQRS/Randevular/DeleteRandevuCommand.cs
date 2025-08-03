using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Randevular
{
	public record DeleteRandevuCommand(int Id) : IRequest<bool>;

	public class DeleteRandevuCommandHandler : IRequestHandler<DeleteRandevuCommand, bool>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;

		public DeleteRandevuCommandHandler(IWebDbContext context, IPrincipal principal)
		{
			_context = context;
			_principal = principal;
		}

		public async Task<bool> Handle(DeleteRandevuCommand request, CancellationToken cancellationToken)
		{
			var identity = await _context.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("Kullanıcı bulunamadı");

			var doktor = await _context.Doktorlar.AsNoTracking()
				.FirstOrDefaultAsync(d => d.IdentityId == identity.Id, cancellationToken)
				?? throw new Exception("Doktor bulunamadı");

			var entity = await _context.Randevular
				.FirstOrDefaultAsync(r => r.Id == request.Id && r.DoktorId == doktor.Id, cancellationToken)
				?? throw new Exception("Randevu bulunamadı veya yetkiniz yok");

			entity.Durum = Status.deleted;

			await _context.SaveChangesAsync(cancellationToken);

			return true;
		}
	}
}
