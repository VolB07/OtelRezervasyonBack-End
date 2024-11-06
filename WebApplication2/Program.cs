using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kestrel sunucu ayarlar�
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB olarak belirledik (iste�e ba�l�)
});

// CORS politikas� ekleyin
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

// Swagger/OpenAPI yap�land�rmas�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context (PostgreSQL) ekleyin
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Geli�tirme modunda Swagger etkinle�tirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS ayarlar�n� devreye al�n
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
