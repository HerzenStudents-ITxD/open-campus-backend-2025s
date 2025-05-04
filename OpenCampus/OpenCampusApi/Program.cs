using OpenCampus.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ������� ������ �����������
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ������������� DbContext _��_ builder.Build()
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(conn));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // React-����������
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // HTTP ? HTTPS
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
