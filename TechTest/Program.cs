using DAL.Data;
using DAL.Data.Interface;
using DAL.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = "Server=localhost,1433;Initial Catalog=taskAPI;Persist Security Info=False;User ID=sa;Password=testpass123@;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
builder.Services.AddDbContext<TaskContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddCors(o => o.AddPolicy("AllowCors", builder =>
{
    builder.WithOrigins("http://localhost:3000")
       .WithHeaders("Authorization", "Content-Type")
       .AllowAnyMethod();
    builder.WithOrigins("http://localhost:4200")
       .WithHeaders("Authorization", "Content-Type")
       .AllowAnyMethod();

}));
builder.Services.AddAuthentication("Bearer").AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes("e10a7138e95a731f5412fa612f60e9efad778eb31779b4bf6e5025bf3f744ab9")
            ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapGet("/", () => "Index");
app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();

app.UseCors("AllowCors");
app.MapControllers();

app.Run();
