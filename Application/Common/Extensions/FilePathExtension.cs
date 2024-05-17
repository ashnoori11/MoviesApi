namespace Application.Common.Extensions;

public static class FilePathExtension
{
    public static string ExtractFileNameFromUrl(this string url)
    {
        string[] splitedUrl = url.Split('.');
        if (splitedUrl.Length == 1)
        {
            string[] splitedRemainUrl = splitedUrl[0].Split("/");
            string theResult = splitedRemainUrl[splitedRemainUrl.Length -1];
            return $"{theResult}.{splitedUrl[0]}";
        }
        else
        {
            return string.Empty;
        }
    }
}
