
using ExchangeRatesAssignment.Api.profiles;
using ExchangeRatesAssignment.Core.Interfaces;
using ExchangeRatesAssignment.Infrastructure.Data;
using ExchangeRatesAssignment.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPartnerRatesRepository, PartnerRatesRepository>();
builder.Services.AddScoped<IPartnerRatesService, PartnerRatesService>();
builder.Services.AddScoped<ICountryCodeService, CountryCodeService>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

{
    var services = builder.Services;
    services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
