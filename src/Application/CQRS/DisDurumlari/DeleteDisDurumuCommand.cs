using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;
using SampleProjectInterns.Entities.Common; // Status enum burada varsayalım

namespace Application.CQRS.DisDurumlari
{
	public record DeleteDisDurumuCommand(int Id) : IRequest;

	public class DeleteDisDurumuCommandHandler : IRequestHandler<DeleteDisDurumuCommand>
	{
		private readonly IWebDbContext _webDbContext;

		public DeleteDisDurumuCommandHandler(IWebDbContext webDbContext)
		{
			_webDbContext = webDbContext;
		}

		public async Task<Unit> Handle(DeleteDisDurumuCommand request, CancellationToken cancellationToken)
		{
			var entity = await _webDbContext.DisDurumlari
				.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

			if (entity == null)
				throw new Exception("DisDurumu kaydı bulunamadı.");

			// Soft delete: Status'u Deleted yap
			entity.Status = Enums.Status.deleted;

			// İstersen burada güncelleme tarihini de set edebilirsin.

			await _webDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
