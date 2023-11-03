using TechnicalAssignment.DBContext;
using TechnicalAssignment.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<InMemoryDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "InMemoryDb"));

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<OrderRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
