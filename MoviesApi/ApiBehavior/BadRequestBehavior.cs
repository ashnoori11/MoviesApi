using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.ApiBehavior;

public class BadRequestBehavior
{
    public static void Parse(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var response = new List<string>();
            foreach (var key in context.ModelState.Keys)
            {
                if (!context.ModelState.ContainsKey(key))
                    continue;

                foreach (var error in context.ModelState[key].Errors)
                {
                    response.Add($"{key}: {error.ErrorMessage}");
                }
            }

            return new BadRequestObjectResult(response);
        };
    }
}
