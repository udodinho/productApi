using Product.Domain.Entities;
using Product.Infrastructure.Repository.Contract;

namespace Product.Infrastructure.Repository.Implementation
{
    public class UserRepository : Repository<User>, IUsersRepository
    {
        public UserRepository(ProductContext appDbContext) : base(appDbContext)
        {

        }
    }
}
