using LeaveManagementSystem.Data;
using LeaveManagementSystem.Services.LeaveAllocations;
using LeaveManagementSystem.Services.LeaveTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// the service is registered in the IOC(Inversion of Control) container and it's usable by other classes
builder.Services.AddScoped<ILeaveTypesService, LeaveTypesService>(); //ILeaveTypesServices is the contract and LeaveTypesSerivces is the implementation
//builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>();

builder.Services.AddHttpContextAccessor();
// it will check the entire assembly(folder structure) that looks like an automapper profile and automatically register
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
