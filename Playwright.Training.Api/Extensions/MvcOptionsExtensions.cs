using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace Playwright.Training.Api.Extensions;

public static class MvcOptionsExtensions
{
    public static MvcOptions InsertJsonPatchInputFormatter(this MvcOptions options, int index)
    {
        ServiceProvider serviceProvider = new ServiceCollection() 
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();
        NewtonsoftJsonPatchInputFormatter inputFormatter = serviceProvider
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
        options.InputFormatters.Insert(index, inputFormatter);
        return options;
    }
}