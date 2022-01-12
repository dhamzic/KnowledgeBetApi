using KnowledgeBet.API.HubConfig;
using KnowledgeBet.API.Services;
using KnowledgeBet.Core.Interfaces;
using KnowledgeBet.Infrastructure;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
    .WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        assembly => assembly.MigrationsAssembly(typeof(QuizDbContext).Assembly.FullName));

});

builder.Services.AddScoped<IKnowledgeBetService, KnowledgeBetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChartHub>("/chart");

app.Run();
