using System.Text;
using CryptoProj.API;
using CryptoProj.API.Endpoints;
using CryptoProj.API.Middlewares;
using CryptoProj.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddTransient<GlobalExceptionHandler>();

builder.Services.AddMemoryCache();
//builder.Services.AddHostedService<TestHostedService>();
//builder.Services.AddHostedService<CryptoAnalysisHostedService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]!))
        };
    });

builder.Services.AddAuthorization(opt => opt
    .AddPolicy("Admin", 
        policy => policy
            .RequireClaim("role", "Admin")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<GlobalExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication(); // хто ти
app.UseAuthorization();  // що ти можеш

app.MapControllers();



/*app.MapGet("api/v1/news", ()
    =>
{
    Console.WriteLine("Hello World!");
    return "News";
});

app.MapPost("api/v1/news/{id}", async ([FromBody] News news, [FromRoute] int id) =>
{
    return Results.Ok(news);
});

app.MapPut("api/v1/news", (HttpContext ctx, CryptoContext context, News news) =>
    {
        context.Update(news);
        return Results.Ok(news);
    })
    .RequireAuthorization()
    .Produces(200)
    .WithSummary("news update endpoint");*/

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path);
    await next(context);
    Console.WriteLine(context.Response.StatusCode);
});

app.MapNewsEndpoints();

app.Run();