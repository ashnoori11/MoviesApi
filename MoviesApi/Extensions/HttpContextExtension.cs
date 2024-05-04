namespace MoviesApi.Extensions;

public static class HttpContextExtension
{
    public static async Task AddHeaderAsync(this HttpContext httpContext,object value,string headerName)
    {
        ArgumentNullException.ThrowIfNull(nameof(httpContext));

        if(headerName is null || string.IsNullOrWhiteSpace(headerName))
            throw new ArgumentNullException($"invalid {nameof(headerName)} with invalid {nameof(value)} detected");

        httpContext.Response.Headers.Append(headerName, value.ToString());
        await Task.CompletedTask;
    }
}
