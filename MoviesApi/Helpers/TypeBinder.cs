using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MoviesApi.Helpers;

public class TypeBinder<T> : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var propertyName = bindingContext.ModelName;
        var value = bindingContext.ValueProvider.GetValue(propertyName);

        if (value == ValueProviderResult.None)
            await Task.CompletedTask;
        else if (string.IsNullOrWhiteSpace(value.FirstValue))
            await Task.CompletedTask;
        else
        {
            try
            {
                var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>(value.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(deserializedValue);
            }
            catch (Exception exp)
            {
                bindingContext.ModelState.TryAddModelError(propertyName,$"the given value is not of the correct type - exception message : {exp.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
