using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Product.Domain.Entities;
using Product.Infrastructure.Repository.Contract;
using Product.Infrastructure.Repository.Implementation;

namespace Product.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private readonly ProductContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly Lazy<IProductRepository> _products;
        private readonly Lazy<IUsersRepository> _users;




        public UnitOfWork(ProductContext appDbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _products = new Lazy<IProductRepository>(() => new ProductRepository(appDbContext));
            _users = new Lazy<IUsersRepository>(() => new UserRepository(appDbContext));

        }

        public IProductRepository Products => _products.Value;
        public IUsersRepository Users => _users.Value;


        public async Task SaveChangesAsync()
        {
            try
            {
                TrackAddedEntities();
                TrackModifiedEntities();
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}: \n {ex.StackTrace}");
                throw;
            }
        }

        private void TrackAddedEntities()
        {
            var entries = _appDbContext.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added);
            foreach (var entry in entries)
            {
                entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;


                if (entry.Property(x => x.Id).CurrentValue == string.Empty)
                    entry.Property(x => x.Id).CurrentValue = Guid.NewGuid().ToString();
            }
        }

        private void TrackModifiedEntities()
        {
            var entries = _appDbContext.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
            }
        }

    }
}
