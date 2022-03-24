using Blog.Data;
using Blog.Models;
using Blog.Services;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = ConnectionService.GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(s =>
{
    OpenApiInfo openApiInfo = new()
    {
        Title = "Atlas Blog API",
        Version = "v1",
        Description = "Candidate API for the Blog",
        Contact = new()
        {
            Name = "Nick Seyler",
            Url = new("https://nickseyler-portfolio.netlify.app/")
        },
        License = new()
        {
            Name = "API License",
            Url = new("https://nickseyler-portfolio.netlify.app/")
        }
    };
    s.SwaggerDoc(openApiInfo.Version, openApiInfo);

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});

builder.Services.AddTransient<DataService>();
builder.Services.AddScoped<IImageService, BasicImageService>();
builder.Services.AddTransient<IEmailSender, BasicEmailService>();
builder.Services.AddTransient<SlugService>();
builder.Services.AddTransient<SearchService>();

var app = builder.Build();

//When calling a service from this middleware, we need an instance of IServiceScope
var scope = app.Services.CreateScope();
var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
await dataService.SetupDbAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API");
    s.InjectStylesheet("~/css/SwaggerUI.css");
    s.InjectJavascript("~/js/SwaggerUI.js");

    if (!app.Environment.IsDevelopment())
    {
        s.RoutePrefix = "";
    }
});

app.MapControllerRoute(
    name: "custom",
    pattern: "PostDetails/{slug}",
    defaults: new {controller = "BlogPosts", action="Details"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
