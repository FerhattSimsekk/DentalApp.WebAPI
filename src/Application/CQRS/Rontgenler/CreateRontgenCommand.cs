using Application.Dtos.Rontgenler.Request;
using Application.Dtos.Rontgenler.Response;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Security.Principal;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Rontgenler
{
	public record CreateRontgenCommand(CreateRontgenDto Rontgen) : IRequest<RontgenDto>;

	public class CreateRontgenCommandHandler : IRequestHandler<CreateRontgenCommand, RontgenDto>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public CreateRontgenCommandHandler(IWebDbContext context, IPrincipal principal, IStorageProvider storage)
		{
			_context = context;
			_principal = principal;
			_storage = storage;
		}

		public async Task<RontgenDto> Handle(CreateRontgenCommand request, CancellationToken cancellationToken)
		{
			// Giriş yapan kullanıcıyı bul
			var identity = await _context.Identities.AsNoTracking()
				.FirstOrDefaultAsync(i => i.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new NotFoundException("Kullanıcı bulunamadı", "identity");

			// Hasta var mı kontrol et
			var hasta = await _context.Hastalar
				.Include(h => h.Identity)
				.FirstOrDefaultAsync(h => h.Id == request.Rontgen.hastaId, cancellationToken)
				?? throw new NotFoundException("Hasta bulunamadı", "hasta");

			// Yeni röntgen nesnesi oluştur
			RontgenGoruntu restoran = new()
			{
				HastaId = request.Rontgen.hastaId,
				CekilmeTarihi=DateTime.UtcNow,
				Status = Status.approved,
			

			};
			
		

			if (request.Rontgen.goruntuYolu is not null)
			{
				await _storage.Put($"{restoran.Id}/{request.Rontgen.goruntuYolu.FileName.Split('.')[0]}.", request.Rontgen?.goruntuYolu?.OpenReadStream(), request.Rontgen.goruntuYolu.FileName.Split('.').Last().ToString(), cancellationToken);
				restoran.GoruntuYolu = $"Shared/{restoran.Id}/{request.Rontgen.goruntuYolu.FileName}";
				await _context.SaveChangesAsync(cancellationToken);
			}
			await _context.RontgenGoruntuler.AddAsync(restoran, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			// Mapper ile dönüş
			return restoran.MapToRontgenDto();
		}
	}
}
