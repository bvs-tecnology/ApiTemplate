using Application.Interfaces;
using Domain.Entities.Dtos;
using Microsoft.Extensions.Logging;
using MassTransit;

namespace Application.Services;

public class TestService(ILogger<TestService> logger, IBus bus) : ITestService
{
    public async Task<TestConsumerDto> TestExchange()
    {
        logger.LogInformation("TestExchange: Started");
        var testDto = new TestConsumerDto(Guid.NewGuid());
        logger.LogInformation("TestExchange: Sending test {id} to bus", testDto.Id);
        await bus.Publish(testDto);
        logger.LogInformation("TestExchange: test {id} sent to bus", testDto.Id);
        logger.LogInformation("TestExchange: Completed");
        return testDto;
    }
}