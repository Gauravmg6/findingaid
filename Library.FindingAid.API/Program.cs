using Library.FindingAid.API.DataAccess;
using Library.FindingAid.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
                          builder.WithOrigins("https://tools.library.pfw.edu").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
                          builder.WithOrigins("https://sites.library.pfw.edu").AllowAnyHeader().AllowCredentials().AllowAnyMethod();
                      });
});

#region Dependency Registrstion

builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();
builder.Services.AddTransient<IMockDataCreation, MockDataCreation>();

#endregion

builder.Services.AddTransient<IApplicationDbContext>(s => new ApplicationDbContext(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("corsapp");
app.UseAuthorization();
app.MapControllers();

app.Run();
