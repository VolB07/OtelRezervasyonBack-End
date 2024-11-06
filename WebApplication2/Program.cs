using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kestrel sunucu ayarlarý
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB olarak belirledik (isteðe baðlý)
});

// CORS politikasý ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Angular frontend adresi
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

// Controller hizmetlerini ekleyin
builder.Services.AddControllers();

// Swagger/OpenAPI yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context (PostgreSQL) ekleyin
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Geliþtirme modunda Swagger etkinleþtirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS ayarlarýný devreye alýn
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
