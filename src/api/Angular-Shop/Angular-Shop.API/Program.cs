using Angular_Shop.API.Client;
using Angular_Shop.API.Configurations;
using Angular_Shop.API.Filters;
using Angular_Shop.API.Middlewares;
using Angular_Shop.Data.Data;
using Angular_Shop.Domain.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CurrencyClient>();
builder.Services.Configure<ExchangeClientOptions>(builder.Configuration.GetSection(ExchangeClientOptions.Section));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.ConfigureRepositories();
builder.Services.ConfigureSupervisors();
builder.Services.ConfigureValidators();
builder.Services.AddAPILogging();
builder.Services.AddCORS();
builder.Services.AddVersioning();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();