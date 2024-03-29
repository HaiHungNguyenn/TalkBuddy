using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Data;
using TalkBuddy.DAL.Implementations;
using TalkBuddy.DAL.Interfaces;
using TalkBuddy.Presentation.Extensions;
using TalkBuddy.Presentation.Middleware;
using TalkBuddy.Presentation.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//Ignore cycle in json
builder.Services.AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<TalkBuddyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.RegisterServices();
builder.Services.AddSingleton<PresenceTracker>(); 

builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
EnsureMigrate(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseMiddleware<AuthMiddleware>();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapHub<PresenceHub>("/presence");
    endpoints.MapHub<NotificationHub>("/notification");
    endpoints.MapHub<MediaHub>("/media");

});

app.Run();

void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TalkBuddyContext>();
    context.Database.Migrate();
}
