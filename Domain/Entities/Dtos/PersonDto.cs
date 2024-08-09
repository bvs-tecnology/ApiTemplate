using Domain.Entities.Enums;

namespace Domain.Entities.Dtos;

public class PersonDto(
    Guid id,
    string name,
    string email,
    DateTime? birthday,
    string? personalCode,
    Gender gender,
    string? phone,
    IEnumerable<UserDto> users
) {
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public DateTime? Birthday { get; set; } = birthday;
    public string? PersonalCode { get; set; } = personalCode;
    public Gender Gender { get; set; } = gender;
    public string? Phone { get; set; } = phone;
    public IEnumerable<UserDto> Users { get; set; } = users;

    public static implicit operator PersonDto(Person person)
    {
        return new PersonDto(
            person.Id,
            person.Name,
            person.Email,
            person.Birthday,
            person.PersonalCode,
            person.Gender,
            person.Phone,
            person.Users.Select(x => (UserDto)x)
        );
    }
}