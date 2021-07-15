using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ILinkRepository
    {

        public Task Add(Link l);
        public Task<Link> Get(string id);

    }
}
