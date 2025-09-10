using JustBeeWeb.Options;
using JustBeeWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Ajouter le support des contrôleurs pour l'API

// Configure Brevo options
builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

// Register services
builder.Services.AddHttpClient<EmailService>();
builder.Services.AddHttpClient<VilleDataService>(); // Nouveau service pour les données de villes
builder.Services.AddSingleton<VilleService>();
builder.Services.AddSingleton<VilleDataService>();
builder.Services.AddSingleton<AlveoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers(); // Ajouter le routage pour les contrôleurs API

app.Run();
