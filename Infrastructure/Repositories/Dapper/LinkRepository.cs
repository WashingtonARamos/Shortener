using Dapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Dapper
{
    public class LinkRepository : ILinkRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;

        private string ConnectionString
        {
            get
            {
                if (_connectionString != null)
                    return _connectionString;
                _connectionString = _configuration["Dapper:ConnectionString"];
                return _connectionString;
            }

        }

        public LinkRepository(IConfiguration configuration) => _configuration = configuration;

        public async Task Add(Link l)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var query = "INSERT INTO Links VALUES(@Id, @OriginalUrl)";
                    await connection.ExecuteAsync(query, l);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<Link> Get(string id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var query = "SELECT * FROM Links WHERE Id = @Id";
                    Link link = await connection.QuerySingleOrDefaultAsync<Link>(query, new { Id = id });
                    return link;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
