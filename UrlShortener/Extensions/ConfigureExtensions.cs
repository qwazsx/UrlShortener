using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace UrlShortener.Extensions
{
    public static class ConfigureExtensions
    {
        public static void AddGlobalExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var ex = JsonSerializer.Serialize(error.Error.Message);
                        await context.Response.WriteAsync(ex);
                    }
                });
            });
        }
    }
}
