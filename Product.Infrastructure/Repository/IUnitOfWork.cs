using Product.Infrastructure.Repository.Contract;

namespace Product.Infrastructure.Repository
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IUsersRepository Users { get; }
        Task SaveChangesAsync();

    }
}
