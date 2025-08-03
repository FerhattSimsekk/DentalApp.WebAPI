using Application.Dtos.DisDurumlari;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleProjectInterns.Entities;

namespace Application.CQRS.DisDurumlari;

public record UpdateDisDurumuCommand(int Id, DisDurumuCreateDto DisDurumu) : IRequest;

public class UpdateDisDurumuCommandHandler : IRequestHandler<UpdateDisDurumuCommand>
{
	private readonly IWebDbContext _webDbContext;

	public UpdateDisDurumuCommandHandler(IWebDbContext webDbContext)
	{
		_webDbContext = webDbContext;
	}

	public async Task<Unit> Handle(UpdateDisDurumuCommand request, CancellationToken cancellationToken)
	{
		var existing = await _webDbContext.DisDurumlari
			.Include(x => x.UygulananDisler)
				.ThenInclude(y => y.UygulananYuzeyler)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (existing == null)
			throw new Exception("Kayıt bulunamadı");

		// Mevcut detayları sil
		_webDbContext.DisDurumlari.Remove(existing);
		await _webDbContext.SaveChangesAsync(cancellationToken);

		// Yeni entity'yi oluştur
		var updated = new DisDurumu
		{
			HastaId = request.DisDurumu.HastaId,
			Tarih=request.DisDurumu.tarih.ToUniversalTime(),
			Status=request.DisDurumu.Status,
			DoktorNotu=request.DisDurumu.doktorNotu,
			UygulananDisler = request.DisDurumu.UygulananDisler.Select(d => new DisDurumDetay
			{
				Dis = d.Dis,
				Durum = d.Durum,
				UygulananYuzeyler = d.Yuzeyler.Select(y => new DisDurumYuzey
				{
					Yuzey = y
				}).ToList()
			}).ToList()
		};

		await _webDbContext.DisDurumlari.AddAsync(updated, cancellationToken);
		await _webDbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
