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
	public record UpdateRontgenCommand(int Id, UpdateRontgenDto Rontgen) : IRequest<RontgenDto>;

	public class UpdateRontgenCommandHandler : IRequestHandler<UpdateRontgenCommand, RontgenDto>
	{
		private readonly IWebDbContext _context;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;

		public UpdateRontgenCommandHandler(IWebDbContext context, IPrincipal principal, IStorageProvider storage)
		{
			_context = context;
			_principal = principal;
			_storage = storage;
		}

		public async Task<RontgenDto> Handle(UpdateRontgenCommand request, CancellationToken cancellationToken)
		{
			var rontgen = await _context.RontgenGoruntuler
				.Include(x => x.Hasta)
				.ThenInclude(h => h.Identity)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (rontgen == null)
				throw new NotFoundException("Röntgen görüntüsü bulunamadı", "Röntgen");

			// Hasta ID değiştirilebilirse:
			rontgen.HastaId = request.Rontgen.hastaId;

			// Yeni dosya geldiyse
			if (request.Rontgen.goruntuYolu is not null)
			{
				var dosyaAdi = request.Rontgen.goruntuYolu.FileName;
				var uzanti = dosyaAdi.Split('.').Last();
				var ad = dosyaAdi.Split('.')[0];

				await _storage.Put($"{rontgen.Id}/{ad}.", request.Rontgen.goruntuYolu.OpenReadStream(), uzanti, cancellationToken);
				rontgen.GoruntuYolu = $"Shared/{rontgen.Id}/{dosyaAdi}";
			}

			rontgen.Status = request.Rontgen.status; // Güncelleme sonrası aktif olsun

			await _context.SaveChangesAsync(cancellationToken);

			return rontgen.MapToRontgenDto();
		}
	}
}
