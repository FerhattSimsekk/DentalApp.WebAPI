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

namespace Application.CQRS.Hastalar
{
	public record DeleteHastaCommand(long Id) : IRequest;

	public class DeleteHastaCommandHandler : IRequestHandler<DeleteHastaCommand>
	{
		private readonly IWebDbContext _webDbContext;
		private readonly IPrincipal _principal;

		public DeleteHastaCommandHandler(IWebDbContext webDbContext, IPrincipal principal)
		{
			_webDbContext = webDbContext;
			_principal = principal;
		}

		public async Task<Unit> Handle(DeleteHastaCommand request, CancellationToken cancellationToken)
		{
			




			var restoran = await _webDbContext.Hastalar.FirstOrDefaultAsync(id => id.Id == request.Id, cancellationToken)
				?? throw new NotFoundException($"Restoran Not found", "Restoran");


			restoran.Status = Status.deleted;
			await _webDbContext.SaveChangesAsync(cancellationToken);


			return Unit.Value;
		}
	}
}
