using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uppgift_14.Data;
using Uppgift_14.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// added ApplicationUser as the type
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false; // <-- changed to false

    // configure password requirements to be more relaxed
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 1;
}) 
    .AddRoles<IdentityRole>() // added this line, default Microsoft implementation
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// seed admin user
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();

    //db.Database.EnsureDeleted();
    //db.Database.Migrate();

    // stored in secrets.json, to access right click on project
    // and select: Manage User Secrets
    var adminPW = builder.Configuration["AdminPass"];

    try {
        await SeedData.InitAsync(db, services, adminPW);
    }
    catch (Exception ex) {
        Console.WriteLine(ex.Message);
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
}
else {
    //app.UseExceptionHandler("GymClasses/Error");
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GymClasses}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
