using Domain.Entities.Enums;

namespace Domain.Entities;

public class Person(string name, string email, DateTime? birthday, string? personalCode, Gender gender, string? phone) : BaseEntity
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public DateTime? Birthday { get; set; } = birthday;
    public string? PersonalCode { get; set; } = personalCode;
    public Gender Gender { get; set; } = gender;
    public string? Phone { get; set; } = phone;
    
    #region Mapping
    public virtual List<User> Users { get; set; } = [];
    #endregion
}