using BillTracker.Api.Data;
using BillTracker.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

// This file does not change when a feature is added. Features are IEndpointModule classes
// under Features/, found and mapped at startup. The DbContext registration below is the
// one infrastructure exception: features map entities beside themselves.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
builder.Services.AddCors(o => o.AddPolicy("Web", policy =>
    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddEndpointModules(builder.Configuration, typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors("Web");
app.MapEndpointModules();
app.Run();

// Lets the tests host the real application in memory.
public partial class Program
{
}
