using AccountService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountService.IntegrationTests.Base;

public class EmptyDbContext(DbContextOptions<ApplicationDbContext> options) : ApplicationDbContext(options);
