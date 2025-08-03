using Application.Dtos.DisDurumlari;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;

namespace Application.CQRS.DisDurumlari
{
	public record CreateDisDurumuCommand(DisDurumuCreateDto DisDurumu) : IRequest<int>;

	public class CreateDisDurumuCommandHandler : IRequestHandler<CreateDisDurumuCommand, int>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public CreateDisDurumuCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<int> Handle(CreateDisDurumuCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("Kullanıcı bulunamadı");

			var doktor = await _webDbContext.Doktorlar
				.AsNoTracking()
				.FirstOrDefaultAsync(d => d.IdentityId == identity.Id, cancellationToken)
				?? throw new Exception("Doktor bulunamadı");

			var dto = request.DisDurumu;

			// Ana DisDurumu Entity'si
			var disDurumu = new DisDurumu
			{
				HastaId = dto.HastaId,
				Tarih=DateTime.UtcNow,
				Status=SampleProjectInterns.Entities.Common.Enums.Status.approved,
				DoktorNotu=dto.doktorNotu,
				UygulananDisler = dto.UygulananDisler.Select(detayDto => new DisDurumDetay
				{
					Dis = detayDto.Dis,
					Durum = detayDto.Durum,
					UygulananYuzeyler = detayDto.Yuzeyler.Select(y =>
						new DisDurumYuzey { Yuzey = y }).ToList()
				}).ToList()
			};

			await _webDbContext.DisDurumlari.AddAsync(disDurumu, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);

			return disDurumu.Id;
		}
	}
}
