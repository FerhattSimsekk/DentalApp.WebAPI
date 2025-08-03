using Application.Dtos.TedaviKaydiDto;
using Application.Interfaces;
using Application.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.TedaviKayitlari
{
    public record GetTedaviKayitlariQuery() : IRequest<List<TedaviKaydiDto>>;

    public class GetTedaviKayitlariQueryHandler : IRequestHandler<GetTedaviKayitlariQuery, List<TedaviKaydiDto>>
    {
        private readonly IWebDbContext _context;

        public GetTedaviKayitlariQueryHandler(IWebDbContext context)
        {
            _context = context;
        }

        public async Task<List<TedaviKaydiDto>> Handle(GetTedaviKayitlariQuery request, CancellationToken cancellationToken)
        {
            var kayitlar = await _context.TedaviKayitlari
                .Include(k => k.UygulananDisler)
                    .ThenInclude(d => d.UygulananYuzeyler)
                 .Include(c=>c.Hasta)
                 .ThenInclude(x=>x.Identity)

				.AsNoTracking()
                .Select(k => k.MapToDto()) // Bunu Mapper'a eklemeliyiz
                .ToListAsync(cancellationToken);

            return kayitlar;
        }
    }
}
