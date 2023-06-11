using Pfm.Api.Helpers;
using Pfm.Core.Helpers;
using Pfm.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettings = builder.Configuration.GetSection("appsettings").Get<AppSettingModel>();
ApplicationSetting.AddSettings(appSettings);

builder.WebHost.UseUrls(AppSetting.BaseUrlProxy);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
