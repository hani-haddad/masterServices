using System.Configuration;
using System.Text;
using CRUDAPI.Helpers;
using CRUDAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SharedModelNamespace.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Enable CORS
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
     .AllowAnyHeader());
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
var dBSettings = new DBSettings();
builder.Configuration.GetSection("AuthDBSettings").Bind(dBSettings);
builder.Services.AddSingleton(dBSettings);
builder.Services.AddSingleton<IAuthDBSettings, AuthDBSettings>();

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var mongoDbSettings = builder.Configuration.GetSection("AuthDBSettings").Get<AuthDBSettings>();
    return new MongoClient(mongoDbSettings.ConnectionString);
});


// // requires using Microsoft.Extensions.Options
// var authDBSettings = builder.Configuration.GetSection("AuthDBSettings");
// builder.Services.Configure<AuthDBSettings>(authDBSettings);

// builder.Services.AddSingleton<IAuthDBSettings>(sp =>
//    sp.GetRequiredService<IOptions<AuthDBSettings>>().Value);

// builder.Services.AddSingleton<IMongoClient>(s =>
//     new MongoClient(builder.Configuration.GetConnectionString("MongoDbConnection")));

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourSecretKey")),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddScoped<IUserService, UserService>();
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
