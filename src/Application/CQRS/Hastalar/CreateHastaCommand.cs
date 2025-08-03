
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Doktorlar.Response;
using Application.Dtos.Hastalar.Request;
using Application.Dtos.Hastalar.Response;
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

namespace Application.CQRS.Hastalar
{
	public record CreateHastaCommand(CreateHastaDto hasta) : IRequest<HastaDto>;
	public class CreateHastaCommandHandler : IRequestHandler<CreateHastaCommand, HastaDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateHastaCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<HastaDto> Handle(CreateHastaCommand request, CancellationToken cancellationToken)
		{

			var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: false, passwordLength: 8);

			var password = "232323";// pwd.Next();
			var salt = BCrypt.Net.BCrypt.GenerateSalt();
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

			var newIdentity = new Identity()
			{
				Email = request.hasta.mail,
				Password = hashedPassword,
				Salt = salt,
				Type = AdminAuthorization.user,//  (identity.Type == AdminAuthorization.admin || identity.Type == AdminAuthorization.moderator) ? (AdminAuthorization)request.Identity.Type : AdminAuthorization.user,
				Name = request.hasta.ad,
				LastName = request.hasta.soyad,
				Status = Status.approved,

			};
			_webDbContext.Identities.Add(newIdentity);
			await _webDbContext.SaveChangesAsync(cancellationToken);


			var hasta = new Hasta
			{DogumTarihi=request.hasta.dogumTarihi.ToUniversalTime(),
			TelefonNo=request.hasta.telefonNo,
			KanGrubu=request.hasta.kanGrubu,
			Cinsiyet= (Gender)request.hasta.cinsiyet,
			Adres=request.hasta.adres,
			Tc=request.hasta.tc,

				
				IdentityId = newIdentity.Id,
				Status = Status.approved// Identity ilişkilendirilmiş olacak
										//Adres = new Adres
										//{
										//	// Adres bilgilerini doldur
										//	CityId = 1,
										//	CountyId = 1,

				//}
			};



			await _webDbContext.Hastalar.AddAsync(hasta, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return hasta.MapToHastaDto();
		}
	}
}
