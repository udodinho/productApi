using Product.Domain.Entities;

namespace Product.Infrastructure.Repository.Contract
{
    public interface IProductRepository : IRepository<ProductItem>
    {
    }
}
