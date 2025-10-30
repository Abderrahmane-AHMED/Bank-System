
using BusinessLogic.Services;
using DataAccess.DbContext.Data;
using DataAccess.Repositories;
using Domain;
using Interfaces.Repositories;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);



#region Entity FrameWork

builder.Services.AddDbContext<BankSystemContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


#endregion

var password = Environment.GetEnvironmentVariable("DB_PASSWORD");


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllersWithViews();





#region Custom Repositories 

builder.Services.AddScoped<IClient, ClientRepository>();
#endregion



#region Coustom Services


builder.Services.AddScoped<IClientService, ClientService>();
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


