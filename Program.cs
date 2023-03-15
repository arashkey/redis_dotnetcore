using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Redis 
builder.Services.AddStackExchangeRedisCache(options =>
           {
               options.Configuration = "127.0.0.1:9191,password=123456";
               options.InstanceName = "";
           });

builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect("127.0.0.1:9191,password=123456"));

#endregion

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
