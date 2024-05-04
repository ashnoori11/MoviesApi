namespace Application.Common.Models;

public class Result<T>
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    internal Result(List<T> dataList, bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Data = dataList;
    }

    internal Result(List<T> dataList, bool succeeded,int totalCount, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Data = dataList;
        TotalCount = totalCount;
    }

    public bool Succeeded { get; init; }
    public string[] Errors { get; init; }
    public List<T> Data { get; set; } = default;
    public int TotalCount { get; set; } = 0;

    public static Result<T> NotFound()
        => new Result<T>(false, Array.Empty<string>());

    public static Result<T> Success()
        => new Result<T>(true, Array.Empty<string>());

    public static Result<T> Success<T>(T data)
        => new Result<T>(new List<T> { data}, true, Array.Empty<string>());

    public static Result<T> Success<T>(List<T> dataList)
        => new Result<T>(dataList, true, Array.Empty<string>());

    public static Result<T> Success<T>(List<T> dataList,int totalCount)
        => new Result<T>(dataList, true, totalCount, Array.Empty<string>());

    public static Result<T> Failure(IEnumerable<string> errors)
    => new Result<T>(false, errors);

    public static Result<T> Failure(string error)
        => new Result<T>(false, new string[] { error });

    public static Result<T> Failure<T>(IEnumerable<string> errors)
        => new Result<T>(false, errors);

    public static Result<T> Failure<T>(string error)
            => new Result<T>(false, new string[] { error });
}

public partial class Result
{
    public Result(bool status, string[] messages)
    {
        Succeeded = status;
        Errors = messages;
    }

    public Result(bool status,bool isNotFound, string messages)
    {
        Succeeded = status;
        Errors = new string[] { messages };
        IsNotFound = isNotFound;
    }

    public Result(bool status, bool isNotFound = false)
    {
        Succeeded = status;
        IsNotFound = isNotFound;
        Errors = Array.Empty<string>();
    }

    public bool Succeeded { get; init; }
    public string[] Errors { get; init; }
    public bool IsNotFound { get; set; } = false;

    public static Result Success()
        => new Result(true, Array.Empty<string>());

    public static Result NotFound()
        => new Result(false, true);

    public static Result NotFound(string message)
    => new Result(false, true,message);

    public static Result Failure(string error)
        => new Result(false, new string[] { error });

    public static Result Failure(string[] errors)
    => new Result(false, errors);

}
