using Domain.Entities;
using Domain.Entities.Dtos;
using Infra.Data.Repositories;
using Tests.Mocks.Entities;
using PushSubscription = Domain.Entities.PushSubscription;

namespace Tests.Repositories;

public class PushSubscriptionRepositoryTests : BaseRepositoryTest<PushSubscriptionRepository, PushSubscriptionMock, PushSubscription>;