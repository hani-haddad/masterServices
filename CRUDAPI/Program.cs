using CRUDAPI.Services;
using SharedModelNamespace.Shared.Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
////Specify the URL and port to listen on all available network interfaces
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(7272); // Bind to all available network interfaces
// });


// Add services to the container.
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
     .AllowAnyHeader());
});
// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
// Register AuthDBSettings
// var authDBSettings = builder.Configuration.GetSection("AuthDBSettings");
// builder.Services.Configure<AuthDBSettings>(authDBSettings);

// builder.Services.AddSingleton<IAuthDBSettings>(sp =>
//     sp.GetRequiredService<IOptions<AuthDBSettings>>().Value);
var authDbSettings = new AuthDBSettings();
builder.Configuration.GetSection("AuthDBSettings").Bind(authDbSettings);
builder.Services.AddSingleton<IAuthDBSettings>(authDbSettings);

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(authDbSettings.ConnectionString));

var appSettingsSection = new AppSettings();
builder.Configuration.GetSection("AppSettings").Bind(appSettingsSection);
builder.Services.AddSingleton<AppSettings>(appSettingsSection);

// configure jwt authentication

var key = Encoding.ASCII.GetBytes(appSettingsSection.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//DI

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<SharedModelNamespace.Shared.DBRepositories.IMongoDbRepository, SharedModelNamespace.Shared.DBRepositories.MongoDbRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
