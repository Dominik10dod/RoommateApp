using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.Services;
using RoommateApp.Db.Context;
using RoommateApp.Web.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ChoreService>();

// 1. Blazor komponensek
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// 2. Adatbázis és BLL szolgáltatások
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<RoommateApp.Web.Client.UsersClient>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new RoommateApp.Web.Client.UsersClient(httpClient)
    {
        ReadResponseAsString = true
    };
});
builder.Services.AddScoped<ApartmentService>();

// 3. Web API, OpenAPI (NSwag) és központi hibakezelés
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "RoommateApp API";
    config.Version = "v1";
});

var app = builder.Build();

// --- AUTOMATIKUS ADATBÁZIS HELYREÁLLÍTÁS (Auto-Seed) ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ha a UserApartments tábla üres, automatikusan összekötjük a meglévő felhasználókat a szobával
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
// -------------------------------------------------------

// 4. Fejlesztői eszközök és NSwag
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

// 5. Biztonsági és hálózati middleware-ek
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();

// 6. Végpontok (Endpoints) bekötése a pipeline végén
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(RoommateApp.Web.Client._Imports).Assembly);

app.Run();