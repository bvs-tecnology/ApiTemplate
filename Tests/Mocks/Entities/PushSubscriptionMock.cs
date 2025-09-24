using Domain.Common.InputModel;
using Domain.Entities;

namespace Tests.Mocks.Entities;

public class PushSubscriptionMock : BaseMock<PushSubscription>
{
    protected override PushSubscription GetEntity(Guid id)
    {
        var inputModel = new PushSubscriptionInputModel
        {
            Endpoint = _faker.Internet.Url(),
            Auth = _faker.Random.String(_faker.Random.Int(10, 20)),
            P256dh = _faker.Random.String(_faker.Random.Int(10, 20))
        };
        return new PushSubscription(inputModel, id);
    }
}