using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Configuration.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.EFCore
{
    public class LinkRepository : ILinkRepository
    {
        private readonly DataContext _data;

        public LinkRepository(DataContext data) => _data = data;

        public async Task Add(Link l)
        {
            _data.Add(l);
            await _data.SaveChangesAsync();
        }

        public async Task<Link> Get(string id) => await _get(_data, id);

        // Compiled queries for extra performance
        private readonly static Func<DataContext, string, Task<Link>> _get =
            EF.CompileAsyncQuery((DataContext data, string id) =>
                (from c in data.Links
                 where c.Id == id
                 select c).FirstOrDefault());
    }
}
