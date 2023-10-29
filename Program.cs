//using Microsoft.AspNetCore.Cors.Infrastructure;
//using MongoDB.Driver;
//using static ShoppingCartController;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// Khởi tạo kết nối đến MongoDB
//var connectionString = "mongodb+srv://admin:admin@cluster0.56wxy95.mongodb.net/";
//var databaseName = "DaTaMonAnNhanh";
//var client = new MongoClient(connectionString);
//var database = client.GetDatabase(databaseName);


////builder.Services.AddSingleton<ICartService, CartService>();



//// Đăng ký đối tượng IMongoDatabase vào DI container
//builder.Services.AddSingleton<IMongoDatabase>(database);

//builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ để lưu trữ session
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Đặt thời gian timeout cho session
//    options.Cookie.HttpOnly = true;
//});


//// ConfigureServices method in Startup.cs
//builder.Services.AddSession();
//builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ để lưu trữ session
//builder.Services.AddControllersWithViews(); // Chắc chắn rằng bạn đã thêm dịch vụ này






//var app = builder.Build();

//app.UseSession(); // Đặt cấu hình session ở đây

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//    // ...
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseSession();
//app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Food}/{action=Index}/{id?}");

//app.Run();


using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<VoucherRepository>();


// Khởi tạo kết nối đến MongoDB
var connectionString = "mongodb+srv://admin:admin@cluster0.56wxy95.mongodb.net/";
var databaseName = "DaTaMonAnNhanh";
var client = new MongoClient(connectionString);
var database = client.GetDatabase(databaseName);

// Đăng ký đối tượng IMongoDatabase vào DI container
builder.Services.AddSingleton<IMongoDatabase>(database);
builder.Services.AddDistributedMemoryCache();


builder.Services.AddMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",

    pattern: "{controller=PhanLoai}/{action=Index}/{id?}");

app.Run();
