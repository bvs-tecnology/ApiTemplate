namespace Domain.Entities.Dtos;

public class TestConsumerDto : BaseEntity
{
    public TestConsumerDto(Guid id)
    {
        CreatedBy = id;
    }
}