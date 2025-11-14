
using BusinessLogic.Services;
using DataAccess.DbContext.Data;
using DataAccess.Repositories;
using DataAccess.Repository;
using Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

#region Cookies

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});
#endregion


#region Entity FrameWork

builder.Services.AddDbContext<BankSystemContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<BankSystemContext>()
.AddDefaultTokenProviders();
#endregion



#region Email Sender

var emailAddress = builder.Configuration["Email:Address"];
var emailPassword = builder.Configuration["Email:Password"];

builder.Services.AddScoped<IEmailSender>(provider =>
    new GmailEmailSender(emailAddress, emailPassword));
#endregion


var password = Environment.GetEnvironmentVariable("DB_PASSWORD");


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllersWithViews();





#region Custom Repositories 

builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<IClient, ClientRepository>();
builder.Services.AddScoped<IEmployee, EmployeeRepository>();
#endregion



#region Coustom Services

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
#endregion




builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});







builder.Services.AddLogging();

var app = builder.Build();




if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

#region Routing



app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=Home}/{action=Index}/{id?}");

#endregion




app.Run();


