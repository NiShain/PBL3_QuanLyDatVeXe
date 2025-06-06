﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PBL3_QuanLyDatXe.Services.Vnpay;
using PBL3_QuanLyDatXe.Data;
using PBL3_QuanLyDatXe.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

//Connect to VnPayAPI
builder.Services.AddScoped<IVnPayService, VnPayService>();


// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Kiểm tra nếu chưa có tài khoản admin thì tạo mới
    if (!context.Accounts.Any(a => a.role == "Admin"))
    {
        var admin = new Account
        {
            ten = "admin",
            password = "admin123", // Nên mã hóa mật khẩu trong thực tế
            role = "Admin"
        };
        context.Accounts.Add(admin);
        context.SaveChanges();
    }
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
    await next();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
app.Run();
