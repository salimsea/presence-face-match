using Microsoft.OpenApi.Models;
using Pfm.Api.Helpers;
using Pfm.Core.Helpers;
using Pfm.Core.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(
                                "http://localhost:3001",
                                "http://localhost:3000",
                                "http://localhost:5173"
                                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] { }
                }
                });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Asy Api",
        Version = "v1",
        Description = "API untuk komunikasi dengan database"
    });
});


var appSettings = builder.Configuration.GetSection("appsettings").Get<AppSettingModel>();
ApplicationSetting.AddSettings(appSettings);

builder.WebHost.UseUrls(AppSetting.BaseUrlProxy);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();
app.UseCors();

app.PathSetting();
app.Run();
