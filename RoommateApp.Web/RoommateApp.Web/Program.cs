using Microsoft.EntityFrameworkCore;
using Radzen;
using RoommateApp.Bll.Services;
using RoommateApp.Db.Context;
using RoommateApp.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. SZOLGÁLTATÁSOK REGISZTRÁLÁSA (Services Container)
// =========================================================

// CORS Szolgáltatás (Rugalmas Origin beállítással Blazor WASM-hez)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Blazor & Radzen UI komponensek
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Adatbázis (EF Core SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// BLL Szolgáltatások
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChoreService>();
builder.Services.AddScoped<ApartmentService>();

// HTTP Kliens & API Kliensek
builder.Services.AddHttpClient();
builder.Services.AddScoped<RoommateApp.Web.Client.UsersClient>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new RoommateApp.Web.Client.UsersClient(httpClient)
    {
        ReadResponseAsString = true
    };
});

// Web API, OpenAPI (NSwag) és Hibakezelés
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "RoommateApp API";
    config.Version = "v1";
});

var app = builder.Build();

// =========================================================
// 2. AUTOMATIKUS ADATBÁZIS HELYREÁLLÍTÁS (Auto-Seed)
// =========================================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.UserApartments.Any())
    {
        var defaultApartment = db.Apartments.FirstOrDefault();
        if (defaultApartment == null)
        {
            defaultApartment = new RoommateApp.Db.Entities.Apartment
            {
                Name = "Budapesti Lakás",
                RewardThreshold = 5000,
                ResetSchedule = "Sunday 12:00 AM",
                InviteCode = "APT4B-8X92"
            };
            db.Apartments.Add(defaultApartment);
            db.SaveChanges();
        }

        var existingUsers = db.Users.ToList();
        foreach (var u in existingUsers)
        {
            db.UserApartments.Add(new RoommateApp.Db.Entities.UserApartment
            {
                UserId = u.Id,
                ApartmentId = defaultApartment.Id,
                IsAdmin = (u.Id == 1 || u.Email.Contains("dodo") || u.Name.Contains("Dominik"))
            });
        }
        db.SaveChanges();
    }
}

// =========================================================
// 3. MIDDLEWARE PIPELINE (SZIGORÚ SORREND!)
// =========================================================

// A. Fejlesztői eszközök & Kivételkezelés
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseOpenApi();
    app.UseSwaggerUi();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// B. HTTPS és Hálózati átirányítások
app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

// C. Utválasztás (Routing)
app.UseRouting();

// D. CORS (Kizárólag a UseRouting után és a UseAuthorization / MapControllers előtt!)
app.UseCors("AllowAll");

// E. Autentikáció & Autorizáció & Biztonság
app.UseAuthorization();
app.UseAntiforgery();

// F. Statikus fájlok
app.MapStaticAssets();

// G. Végpontok (Controllers & Blazor App)
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RoommateApp.Web.Client._Imports).Assembly);

app.Run();