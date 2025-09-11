using JustBeeInfrastructure.Context;
using JustBeeInfrastructure.Data;
using JustBeeInfrastructure.Repositories;
using JustBeeWeb.Options;
using JustBeeWeb.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure global JSON options with proper type info resolvers
var jsonOptions = new JsonSerializerOptions
{
    TypeInfoResolver = JsonTypeInfoResolver.Combine(
        EmailSerializationContext.Default,
        MapBeeSerializationContext.Default
    ),
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false
};

// Configure global JSON options for the application
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.TypeInfoResolver = jsonOptions.TypeInfoResolver;
    options.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
    options.WriteIndented = jsonOptions.WriteIndented;
});

// Configure API controllers with proper options for .NET 9
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
    options.EnableEndpointRouting = true;
    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.TypeInfoResolver = jsonOptions.TypeInfoResolver;
    options.JsonSerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
    options.JsonSerializerOptions.WriteIndented = jsonOptions.WriteIndented;
});

// Configure API Explorer with enhanced metadata support for .NET 9
builder.Services.AddEndpointsApiExplorer();

// Add HybridCache with custom JSON serializer configuration
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(2)
    };
});

// Configure a custom JSON serializer for HybridCache
builder.Services.AddSingleton<JsonSerializerOptions>(provider => jsonOptions);

// Keep memory cache for backward compatibility if needed
builder.Services.AddMemoryCache();

// Add response caching
builder.Services.AddResponseCaching();

// Configure Entity Framework with performance optimizations and proper DbContext scoping
builder.Services.AddDbContext<JustBeeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
    {
        sqlOptions.CommandTimeout(30);
        sqlOptions.EnableRetryOnFailure(3);
    });

    options.EnableSensitiveDataLogging(false);
    options.EnableServiceProviderCaching();
    options.EnableDetailedErrors(builder.Environment.IsDevelopment());
}, ServiceLifetime.Scoped);

// Configure Brevo options
builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

// Register repositories with proper scoping
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IAlveoleRepository, AlveoleRepository>();
builder.Services.AddScoped<IVilleRepository, VilleRepository>();
builder.Services.AddScoped<IDepartementRepository, DepartementRepository>();

// Register services with DI and proper scoping
builder.Services.AddHttpClient<EmailService>();
builder.Services.AddHttpClient<VilleDataService>();
builder.Services.AddScoped<VilleService>();
builder.Services.AddScoped<VilleDataService>();
builder.Services.AddScoped<AlveoleService>();
builder.Services.AddScoped<DepartementService>();
builder.Services.AddScoped<CacheService>();

var app = builder.Build();

// Seed initial data
await DataSeeder.SeedDataAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Remove HTTPS redirection for Azure Web App
// app.UseHttpsRedirection(); 

// Add response caching middleware
app.UseResponseCaching();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();