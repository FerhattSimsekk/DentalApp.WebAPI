using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static SampleProjectInterns.Entities.Common.Enums;

namespace Application.CQRS.TedaviKayitlari
{
	public record DeleteTedaviKaydiCommand(int Id) : IRequest<Unit>;

	public class DeleteTedaviKaydiCommandHandler : IRequestHandler<DeleteTedaviKaydiCommand, Unit>
	{
		private readonly IWebDbContext _webDbContext;

		public DeleteTedaviKaydiCommandHandler(IWebDbContext webDbContext)
		{
			_webDbContext = webDbContext;
		}

		public async Task<Unit> Handle(DeleteTedaviKaydiCommand request, CancellationToken cancellationToken)
		{
			var entity = await _webDbContext.TedaviKayitlari
				.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

			if (entity == null)
				throw new Exception("Tedavi kaydı bulunamadı.");

			entity.Durum = TedaviDurumu.IptalEdildi;

			await _webDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
