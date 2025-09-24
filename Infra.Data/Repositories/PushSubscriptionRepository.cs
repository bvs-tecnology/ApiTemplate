using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infra.Data.Context;

namespace Infra.Data.Repositories;

public class PushSubscriptionRepository(IUnitOfWork unitOfWork) : BaseRepository<PushSubscription>(unitOfWork), IPushSubscriptionRepository;