using AccountService.Infrastructure.Models;
using AccountService.Infrastructure.Repositories.Interfaces;

namespace AccountService.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T: IDatabaseModel
{
    //TODO: сделать
}
