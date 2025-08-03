
using Application.Dtos.Doktorlar.Request;
using Application.Dtos.Doktorlar.Response;
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

namespace Application.CQRS.Doktors
{
	public record CreateDoktorCommand(CreateDoktorDto doktor) : IRequest<DoktorDto>;
	public class CreateDoktorCommandHandler : IRequestHandler<CreateDoktorCommand, DoktorDto>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;
		private readonly IStorageProvider _storage;


		public CreateDoktorCommandHandler(IWebDbContext webDbContext, IPrincipal principal, IStorageProvider storage)
		{
			_webDbContext = webDbContext;
			_principal = principal;
			_storage = storage;
		}

		public async Task<DoktorDto> Handle(CreateDoktorCommand request, CancellationToken cancellationToken)
		{

			var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: false, passwordLength: 8);

			var password = request.doktor.password;// pwd.Next();
			var salt = BCrypt.Net.BCrypt.GenerateSalt();
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

			var newIdentity = new Identity()
			{
				Email = request.doktor.mail,
				Password = hashedPassword,
				Salt = salt,
				Type = AdminAuthorization.moderator,//  (identity.Type == AdminAuthorization.admin || identity.Type == AdminAuthorization.moderator) ? (AdminAuthorization)request.Identity.Type : AdminAuthorization.user,
				Name = request.doktor.ad,
				LastName = request.doktor.soyad,
				Status = Status.approved,

			};
			_webDbContext.Identities.Add(newIdentity);
			await _webDbContext.SaveChangesAsync(cancellationToken);


			var doktor = new Doktor
			{
				UzmanlikAlani=request.doktor.uzmanlikAlani,
				LisansNumarasi=request.doktor.lisansNumarasi,
				IdentityId = newIdentity.Id,
				Status = Status.approved// Identity ilişkilendirilmiş olacak
										//Adres = new Adres
										//{
										//	// Adres bilgilerini doldur
										//	CityId = 1,
										//	CountyId = 1,

				//}
			};



			await _webDbContext.Doktorlar.AddAsync(doktor, cancellationToken);
			await _webDbContext.SaveChangesAsync(cancellationToken);






			return doktor.MapToDoktorDto();
		}
	}
}
