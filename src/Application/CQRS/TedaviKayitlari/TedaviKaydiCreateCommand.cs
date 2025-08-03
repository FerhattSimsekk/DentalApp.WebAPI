
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Doktorlar.Response;
using Application.Dtos.TedaviKaydiDto;
using Application.Interfaces;
using Application.Mappers;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;
using SampleProjectInterns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.TedaviKayitlari
{
	public record CreateTedaviKaydiCommand(TedaviKaydiCreateDto tedaviKaydi) : IRequest<int>;
	public class CreateTedaviKaydiCommandHandler : IRequestHandler<CreateTedaviKaydiCommand, int>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateTedaviKaydiCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<int> Handle(CreateTedaviKaydiCommand request, CancellationToken cancellationToken)
		{
			var identity = await _webDbContext.Identities.AsNoTracking()
				.FirstOrDefaultAsync(identity => identity.Email == _principal.Identity!.Name, cancellationToken)
				?? throw new Exception("User Not Found");
			var doktor = await _webDbContext.Doktorlar.AsNoTracking().FirstOrDefaultAsync(z => z.IdentityId == identity.Id);
			var entity = TedaviKaydiMapper.MapFromDto(request.tedaviKaydi);

			
			decimal toplamUcret = 0;

			foreach (var dis in entity.UygulananDisler)
			{
				var tedavi = await _webDbContext.Tedaviler
								.Where(t => t.Id == dis.TedaviId)
								.Select(t => t.Ucret)
								.FirstOrDefaultAsync(cancellationToken);

				toplamUcret += tedavi * 1; // 1 diş için fiyat
			}
			entity.Tarih = entity.Tarih.ToUniversalTime();
			entity.ToplamUcret = toplamUcret;
			entity.DoktorId = doktor.Id;
			entity.OdemeDurumu = OdemeDurumu.Odenmedi;

			await _webDbContext.TedaviKayitlari.AddAsync(entity, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);

			return entity.Id;






		}
	}
}
