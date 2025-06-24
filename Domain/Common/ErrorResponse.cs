namespace Domain.Common;

public class ErrorResponse
{
    private ErrorResponse(params IEnumerable<string> errors)
    {
        Errors = errors;
    }
    public IEnumerable<string> Errors { get; private set; }

    public static class Factory
    {
        public static ErrorResponse Create(params IEnumerable<string> errors) => new(errors);
    }
}