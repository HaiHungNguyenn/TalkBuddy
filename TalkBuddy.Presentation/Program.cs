using Microsoft.EntityFrameworkCore;
using TalkBuddy.DAL.Data;
using TalkBuddy.Presentation.Extensions;
using TalkBuddy.Service.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<TalkBuddyContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
EnsureMigrate(app);
builder.Services.RegisterServices();

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

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatHub>("/chat");
});

app.MapRazorPages();

app.Run();

void EnsureMigrate(WebApplication webApp)
{
    using var scope = webApp.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TalkBuddyContext>();
    context.Database.Migrate();
}
