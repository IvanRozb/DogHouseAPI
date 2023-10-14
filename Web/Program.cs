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

builder.Services.AddDbContextPool<RepositoryDbContext>(optionsBuilder =>
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

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
                PermitLimit = int.Parse(rateLimitConfiguration["PermitLimit"] ?? "10"),
                AutoReplenishment = bool.Parse(rateLimitConfiguration["AutoReplenishment"] ?? "true"),
                Window = TimeSpan.FromSeconds(int.Parse(rateLimitConfiguration["TimeWindow"] ?? "1"))
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

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRateLimiter();

app.MapControllers();

app.Run();
