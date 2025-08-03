using Application.Dtos.Tedavi.Request;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Tedaviler
{
	public record UpdateTedaviCommand(TedaviUpdateDto tedavi,int tedaviId) : IRequest;

	public class UpdateTedaviCommandHandler : IRequestHandler<UpdateTedaviCommand>
	{
		private readonly IWebDbContext _context;

		public UpdateTedaviCommandHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(UpdateTedaviCommand request, CancellationToken cancellationToken)
		{
			var tedavi = await _context.Tedaviler
				.FirstOrDefaultAsync(t => t.Id == request.tedaviId, cancellationToken)
				?? throw new NotFoundException("Tedavi bulunamadı", "tedavi");

			tedavi.Ad = request.tedavi.Ad;
			tedavi.Aciklama = request.tedavi.Aciklama;
			tedavi.Ucret = request.tedavi.Ucret;
			tedavi.Status = request.tedavi.status;
			

			await _context.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
