using JABlog.Data;
using JABlog.Models;
using JABlog.Services;
using JABlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = DataUtility.GetConnectionString(builder.Configuration) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Custome Services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();


builder.Services.AddMvc();

//Add API configs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JABlog API",
        Version = "v1",
        Description = "Serve up Blog Data using .Net 6 Apis",
        Contact = new OpenApiContact
        {
            Name = "J.Apple",
            Email = "JA999Dev@gmail.com",
            //Url = new Uri("https://www.linkedin.com/in/director-raynor/")
        }
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddCors(obj =>
{
    obj.AddPolicy("DefaultPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();
app.UseCors("DefaultPolicy");

var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);


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

//Add Swagger UI Config
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicAPI v1");
    c.InjectStylesheet("/css/swagger.css");
    c.InjectJavascript("/js/swagger.js");

    c.DocumentTitle = "JABlog Public API";
});



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name:"custom",
    pattern:"Content/{slug}",
    defaults: new { controller = "BlogPosts", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
