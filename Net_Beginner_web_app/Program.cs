using Net_Beginner_web_app.Delegate_Events;
using Net_Beginner_web_app.Interfaces;
using Net_Beginner_web_app.Models;
using Net_Beginner_web_app.Repositories;
using Net_Beginner_web_app.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
}).AddHttpMessageHandler<ApiService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<IDataStore,DataStore>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Configure logging providers
builder.Logging.ClearProviders(); // Optional: Clear default providers
builder.Logging.AddConsole(); // Add Console logger
builder.Logging.AddDebug();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ApiService>();
builder.Services.AddScoped<ApiNotification<DailyTasks>>();
builder.Services.AddScoped<ApiNotification<bool>>();
builder.Services.AddScoped<ApiNotification<List<DailyTasks>>>();
builder.Services.AddSingleton<ITaskDataServices, StoreTaskDataService>();
builder.Services.AddTransient<TaskApiActions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //This middleware adds HTTP Strict Transport Security (HSTS) headers to the response, enforcing HTTPS usage.
    //Even if a user types http://yoursite.com, the browser remembers to upgrade the connection to https://yoursite.com automatically.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
