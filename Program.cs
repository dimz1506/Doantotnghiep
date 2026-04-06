using Microsoft.EntityFrameworkCore;
using Doantotnghiep.Data;
using Doantotnghiep.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Konscious.Security.Cryptography;
using Doantotnghiep.Helpers;
using Doantotnghiep.Models.Entities;
using Doantotnghiep.Repositories.Repository;
using Doantotnghiep.Services.Interfaces;
using Doantotnghiep.Repositories.IRepository;
using Doantotnghiep.Services.Implementations;
using Doantotnghiep.Repositories.Interface;
using Doantotnghiep.Repositories.Implementations;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/DangNhap";
        options.LogoutPath = "/Auth/DangXuat";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("NhanVienOnly", policy => policy.RequireRole("NhanVien"));
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ILoaiDichVuRepository, LoaiDichVuRepository>();
builder.Services.AddScoped<ILoaiDichVuServices, LoaiDichVuServices>();
builder.Services.AddScoped<IDichVuRepository, DichVuRepository>();
builder.Services.AddScoped<IDichVuServices, DichVuServices>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Tạo tài khoản admin nếu chưa tồn tại
    if (!context.TaiKhoans.Any(u => u.TenTaiKhoan == "admin"))
    {
        var salt = Argon2PasswordHelper.GenerateSalt();
        var hashedPassword = Argon2PasswordHelper.HashPasswordAsync("123456", salt)
                                .GetAwaiter().GetResult();

        var adminUser = new TaiKhoan
        {
            TenTaiKhoan = "admin",
            Passwordhash = hashedPassword,
            Salt = salt,
            IdVaiTro = 1,
            Email = "admin12345@gmail.com",
            SoDienThoai = "0123456789",
            TrangThaiTK = true
        };

        context.TaiKhoans.Add(adminUser);
        context.SaveChanges(); // lưu xong mới lấy Id

        context.NhanViens.Add(new NhanVien
        {
            IdTaiKhoan = adminUser.IdTaiKhoan,
            TenNhanVien = "Admin",
            ChuyenMonNV = "Quản lý",
            DiaChiNV = "Ha Noi - Viet Nam",
            TrangThaiNV = Doantotnghiep.Models.Enum.TrangThaiNhanVien.DangLamViec
        });
        context.SaveChanges();
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
