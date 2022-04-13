using ChustaSoft.Tools.SecureConfig;
using ChustaSoft.Tools.SecureConfig.TestApi;

var testApikey = "TestPrivateKey-UseItInASecureWay.20200630";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.SetUpSecureConfig<AppSettings>(testApikey);
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
app.EncryptSettings<AppSettings>(false);

app.Run();
