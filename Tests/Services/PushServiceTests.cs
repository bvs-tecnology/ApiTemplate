using System.Linq.Expressions;
using Application.Services;
using Domain.Common.InputModel;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Infra.Utils.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tests.Mocks.Entities;

namespace Tests.Services;

public class PushServiceTests
{
    private readonly PushService _service;
    private readonly Mock<IPushSubscriptionRepository> _repository = new();
    private readonly Mock<IOptionsSnapshot<PushNotificationConfigs>> _configs = new();
    private readonly Mock<ILogger<PushService>> _logger = new();
    
    private readonly Faker _faker = new();
    private readonly PushSubscriptionMock _subscription = new();
    private readonly string _base64UrlChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

    public PushServiceTests()
    {
        var configs = new PushNotificationConfigs
        {
            PublicKey = _faker.Random.String2(128, _base64UrlChars),
            PrivateKey = _faker.Random.String2(64, _base64UrlChars),
            MailTo = _faker.Internet.Email()
        };
        _configs.Setup(x => x.Value).Returns(configs);
        _service = new PushService(_repository.Object, _configs.Object, _logger.Object);
    }

    private PushSubscriptionInputModel PsInputModel => new()
    {
        Endpoint = _faker.Internet.Url(),
        Auth = _faker.Random.String(50),
        P256dh = _faker.Random.String(50)
    };
    
    [Fact]
    public async Task ShouldSubscribeNewSubscription()
    {
        await _service.Subscribe(PsInputModel, Guid.NewGuid());
        
        _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        _repository.Verify(x => x.InsertAsync(It.IsAny<PushSubscription>()), Times.Once);
    }

    [Fact]
    public async Task ShouldSubscribeWithExistingSubscription()
    {
        var id = Guid.NewGuid();
        _repository.Setup(x => x
            .FindAsync(It.IsAny<Expression<Func<PushSubscription, bool>>>()))
            .ReturnsAsync(_subscription.GetEntity(id));
        await _service.Subscribe(PsInputModel, id);
        
        _repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        _repository.Verify(x => x.InsertAsync(It.IsAny<PushSubscription>()), Times.Never);
    }
    
    [Fact]
    public async Task ShouldTryToSubscribeWithAnEmptyUserId()
    {
        var action = async () => await _service.Subscribe(PsInputModel, Guid.Empty);
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }

    [Fact]
    public async Task ShouldUnsubscribe()
    {
        var id = Guid.NewGuid();
        _repository.Setup(x => x.GetByCreator(id)).ReturnsAsync(_subscription.GetEnumerable(3).ToList());
        await _service.Unsubscribe(id);
        
        _repository.Verify(x => x.DeleteAsync(It.IsAny<PushSubscription>()), Times.Exactly(3));
    }

    [Fact]
    public async Task ShouldTryUnsubscribeWithAnEmptyUserId()
    {
        var action = async () => await _service.Unsubscribe(Guid.Empty);
        await Assert.ThrowsAsync<ArgumentNullException>(action);
    }
}