using System.Text.Json;
using Domain.Common.InputModel;
using Domain.Entities.Dtos;
using Domain.Entities.Dtos.PushNotification;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infra.Utils.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebPush;
using PushSubscription = Domain.Entities.PushSubscription;

namespace Application.Services;

public class PushService(
    IPushSubscriptionRepository repository,
    IOptionsSnapshot<PushNotificationConfigs> pushConfigs,
    ILogger<PushService> logger
) : IPushService
{
    private readonly VapidDetails _vapidDetails = new($"mailto:{pushConfigs.Value.MailTo}", pushConfigs.Value.PublicKey, pushConfigs.Value.PrivateKey);
    public async Task Subscribe(PushSubscriptionInputModel inputModel, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        logger.LogInformation("Subscribing user {UserId} to push", userId);
        
        var existingSubscription = await repository.FindAsync(x => x.CreatedBy == userId && x.Endpoint == inputModel.Endpoint);
        if (existingSubscription != null)
        {
            existingSubscription.Update(inputModel);
            await repository.SaveChangesAsync();
            logger.LogInformation("User {UserId} successfully updated subscription to receive push", userId);
            return;
        }
        
        await repository.InsertAsync(new PushSubscription(inputModel, userId));
        logger.LogInformation("User {UserId} successfully subscribed to push", userId);
    }

    public async Task Unsubscribe(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
        logger.LogInformation("Unsubscribing user {UserId}", userId);
        var subscriptions = await repository.GetByCreator(userId);
        foreach (var subscription in subscriptions)
            await repository.DeleteAsync(subscription);
        logger.LogInformation("User {UserId} unsubscribed from push", userId);
    }

    public async Task SendPush(SendPushInputModel inputModel)
    {
        var subscriptions = await repository.GetAll().ToListAsync();

        var webPushClient = new WebPushClient();

        foreach (var sub in subscriptions)
        {
            try
            {
                var subscription = new WebPush.PushSubscription(sub.Endpoint, sub.P256dh, sub.Auth);
                var payload = JsonSerializer.Serialize(new PushNotificationDto(inputModel));
                await webPushClient.SendNotificationAsync(subscription, payload, _vapidDetails);
            }
            catch (WebPushException ex)
            {
                Console.WriteLine($"Erro ao enviar push: {ex.Message}");
            }
        }
    }
}