using SampleProjectInterns.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IWebDbContext
{
    DbSet<Identity> Identities { get; }
    DbSet<DisDurumu> DisDurumlari { get; } 
    DbSet<City> Cities { get; }
    DbSet<County> Counties { get; }  
    DbSet<Odeme> Odemeler { get; }
    DbSet<DisTipi> DisTipleri { get; }
    DbSet<Doktor> Doktorlar { get; }
	DbSet<DoktorNotu> DoktorNotlari { get; }
	DbSet<Hasta> Hastalar { get; }
	DbSet<Randevu> Randevular { get; }
	DbSet<RontgenGoruntu> RontgenGoruntuler { get; }
	DbSet<Tedavi> Tedaviler { get; }
	DbSet<TedaviDis> TedaviDisler { get; }
	DbSet<TedaviKaydi> TedaviKayitlari { get; }
	DbSet<DisDurumDetay> DisDurumDetaylari { get; }
	DbSet<DisDurumYuzey> DisDurumYuzeyleri { get; }










	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
}
