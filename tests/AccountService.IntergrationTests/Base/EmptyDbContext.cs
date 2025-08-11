using AccountService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountService.IntergrationTests.Base;

public class EmptyDbContext : ApplicationDbContext
{
    public EmptyDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
