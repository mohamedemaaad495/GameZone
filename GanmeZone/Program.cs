var builder = WebApplication.CreateBuilder(args);
// Register The Application DbContext.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No Connection String Was Found!");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();
//Register The Categories Service
builder.Services.AddScoped<ICategoriesService,CategoriesService>();
//Register The Devices Service
builder.Services.AddScoped<IDevicesService,DevicesService>();
//Register The Games Service
builder.Services.AddScoped<IGamesService,GamesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
