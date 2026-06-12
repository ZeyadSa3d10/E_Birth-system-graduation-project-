using E_Birth.Api.ServiceConfigration;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services container.
ServiceConfigrations.ConfigureService(builder.Configuration, builder.Services);


builder.Services.AddControllers();
builder.Services.AddMemoryCache(options => options.SizeLimit=100);
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
//app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Project API V1");
    options.SwaggerEndpoint("/openapi/v2.json", "Project API V2");

    options.EnableDeepLinking();
    options.DisplayRequestDuration();
    options.EnableFilter();
});
}


//app.UseHttpsRedirection();
app.UseRouting();
//app.Use(async (context, next) =>
//{
//    if (context.Request.Method == "OPTIONS")
//    {
//        context.Response.StatusCode = 204;
//        await context.Response.CompleteAsync();
//        return;
//    }

//    await next();
//});

app.UseCors("Open");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
