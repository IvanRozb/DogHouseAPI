using System.Threading.RateLimiting;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using Web.Middleware;
using Web.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = 
            SnakeCaseNamingPolicy.Instance;
    });
    

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();

builder.Services.AddDbContext<RepositoryDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddRateLimiter(options =>
{
    var rateLimitConfiguration = builder.Configuration.GetSection("RateLimit");
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = int.Parse(rateLimitConfiguration["PermitLimit"] ?? 10.ToString()),
                AutoReplenishment = bool.Parse(rateLimitConfiguration["AutoReplenishment"] ?? true.ToString()),
                Window = TimeSpan.FromSeconds(int.Parse(rateLimitConfiguration["TimeWindow"] ?? 1.ToString()))
            });
    });
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
    dataContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRateLimiter();

app.MapControllers();

app.Run();
