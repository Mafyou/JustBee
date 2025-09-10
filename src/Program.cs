using JustBeeWeb.Options;
using JustBeeWeb.Services;
using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Data;
using JustBeeInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Ajouter le support des contrôleurs pour l'API

// Configure Entity Framework
builder.Services.AddDbContext<JustBeeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Brevo options
builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

// Register repositories
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IAlveoleRepository, AlveoleRepository>();
builder.Services.AddScoped<IVilleRepository, VilleRepository>();
builder.Services.AddScoped<IDepartementRepository, DepartementRepository>();

// Register services with DI
builder.Services.AddHttpClient<EmailService>();
builder.Services.AddHttpClient<VilleDataService>(); // Nouveau service pour les données de villes
builder.Services.AddScoped<VilleService>();
builder.Services.AddScoped<VilleDataService>();
builder.Services.AddScoped<AlveoleService>();
builder.Services.AddScoped<DepartementService>();

var app = builder.Build();

// Seed initial data
await DataSeeder.SeedDataAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Remove HTTPS redirection for Azure Web App
// app.UseHttpsRedirection(); 

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers(); // Ajouter le routage pour les contrôleurs API

app.Run();
