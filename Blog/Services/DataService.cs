using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class DataService
    {
        readonly ApplicationDbContext _context;

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SetupDBAsync()
        {
            //Run the Migrations async
            await _context.Database.MigrateAsync();
        }
    }
}
