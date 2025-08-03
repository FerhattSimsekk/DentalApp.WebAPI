using Application.Dtos.Tedavi.Request;
using Application.Dtos.Tedavi.Response;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using SampleProjectInterns.Entities;

namespace Application.CQRS.Tedaviler
{
	public record CreateTedaviCommand(TedaviCreateDto tedavi) : IRequest<TedaviDto>;

	public class CreateTedaviCommandHandler : IRequestHandler<CreateTedaviCommand, TedaviDto>
	{
		private readonly IWebDbContext _context;

		public CreateTedaviCommandHandler(IWebDbContext context)
		{
			_context = context;
		}

		public async Task<TedaviDto> Handle(CreateTedaviCommand request, CancellationToken cancellationToken)
		{
			var entity = new Tedavi
			{
				Ad = request.tedavi.Ad,
				Aciklama = request.tedavi.Aciklama,
				Ucret = request.tedavi.Ucret,
				Status=SampleProjectInterns.Entities.Common.Enums.Status.approved
			};

			_context.Tedaviler.Add(entity);
			await _context.SaveChangesAsync(cancellationToken);

			return entity.MapToTedaviDto();
		}
	}
}
