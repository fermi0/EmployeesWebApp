using api.Interfaces;
using api.Data;
using static Global;
using EmployeesWebApp.Services;
using api.Respository;
using EmployeesWebApp.Controllers;

var AllowOrigins = "_AllowOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StaffDB>();
builder.Services.AddTransient<IStaffRepository, StaffRepository>();
builder.Services.AddTransient<EmployeeService>();
builder.Services.AddTransient<HomeController>();
builder.Services.AddHttpClient<EmployeeService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: AllowOrigins, policy =>
    {
        policy.WithOrigins(api_url, "Http://localhost:7261");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(AllowOrigins);
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
