namespace Domain.Common;

public abstract class BaseResponse
{
    public List<string> Errors { get; set; } = [];
    public bool Success => Errors.Count == 0;

    public void AddError(string error)
        => Errors.Add(error);
}