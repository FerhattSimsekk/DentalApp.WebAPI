using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using System.Threading;
using System.Threading.Tasks;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.Rontgenler
{
	public record DeleteRontgenCommand(int Id) : IRequest;

	public class DeleteRontgenCommandHandler : IRequestHandler<DeleteRontgenCommand>
	{
		private readonly IWebDbContext _context;

		public DeleteRontgenCommandHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(DeleteRontgenCommand request, CancellationToken cancellationToken)
		{
			var rontgen = await _context.RontgenGoruntuler
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (rontgen == null)
				throw new NotFoundException("Röntgen görüntüsü bulunamadı", "röntgen");

			rontgen.Status = Status.deleted;

			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
