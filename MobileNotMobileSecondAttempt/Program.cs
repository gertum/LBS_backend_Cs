using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using MobileNotMobileSecondAttempt.Data; // Replace with your actual namespace
using MobileNotMobileSecondAttempt.Services; // Replace with the namespace where LocationService resides

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Directly test the database connection
try
{
    using var connection = new MySqlConnection(connectionString);
    connection.Open(); // Attempt to open the connection
    Console.WriteLine("Database connection successful!");
    connection.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"Database connection failed: {ex.Message}");
}

// Register AppDbContext for Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)))); // Adjust MySQL version as needed

// Register LocationService for dependency injection
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
