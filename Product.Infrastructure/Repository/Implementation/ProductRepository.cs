using Product.Domain.Entities;
using Product.Infrastructure.Repository.Contract;

namespace Product.Infrastructure.Repository.Implementation
{
    public class ProductRepository : Repository<ProductItem>, IProductRepository
    {
        public ProductRepository(ProductContext appDbContext) : base(appDbContext)
        {

        }
    }
}
