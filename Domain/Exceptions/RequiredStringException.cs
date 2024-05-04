namespace Domain.Exceptions;

public class RequiredStringException : Exception
{
    public RequiredStringException() : base() { }
    public RequiredStringException(string message) : base(message) { }
    public RequiredStringException(string message, Exception innerException) : base(message, innerException) { }

    public static void ThrowIfNullOrEmpty(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredStringException("string filed is required and can not be null or empty");
    }

    public static void ThrowIfNullOrEmpty(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredStringException($"{fieldName} is required - error : can not be null or empty");
    }

    public static void ThrowIfNullOrEmpty(string value,string fieldName ,string message)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new RequiredStringException($"{fieldName} is required - error : {message}");
    }
}