using Domain.Entities;
using Domain.Repositories;

namespace Infra.Data.Repository;

public class PeopleRepository(IUnitOfWork unitOfWork) : BaseRepository<Person>(unitOfWork), IPeopleRepository
{
    
}