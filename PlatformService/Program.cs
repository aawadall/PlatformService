using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// check if production or development
var isProduction = builder.Environment.IsProduction();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>(opt => 
{
    if (isProduction)
    {
        System.Console.WriteLine($"Using SQL Server Db [{builder.Configuration.GetConnectionString("PlatformsConnection")}]");
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConnection"));
    }
    else
    {
        System.Console.WriteLine("Using In-Memory Database");
        opt.UseInMemoryDatabase("InMem");
    }
});

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Title = "PlatformService",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


PrepDb.PrepPopulation(app, app.Environment.IsProduction() );

app.Run();
