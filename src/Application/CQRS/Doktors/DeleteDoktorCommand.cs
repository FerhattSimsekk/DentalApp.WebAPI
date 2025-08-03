using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Doktors
{
	public record DeleteDoktorCommand(long Id) : IRequest;

	public class DeleteDoktorCommandHandler : IRequestHandler<DeleteDoktorCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public DeleteDoktorCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(DeleteDoktorCommand request, CancellationToken cancellationToken)
		{
			




			var restoran = await _webDbContext.Doktorlar.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
				?? throw new NotFoundException($"Restoran Not found", "Restoran");


			restoran.Status = Status.deleted;
			await _webDbContext.SaveChangesAsync(cancellationToken);


			return Unit.Value;
		}
	}
}
