using Domain.Entities.Dtos;

namespace Application.Interfaces;

public interface ITestService
{
    Task<TestConsumerDto> TestExchange();
}