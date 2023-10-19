using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Khởi tạo kết nối đến MongoDB
var connectionString = "mongodb+srv://admin:admin@cluster0.56wxy95.mongodb.net/";
var databaseName = "DaTaMonAnNhanh";
var client = new MongoClient(connectionString);
var database = client.GetDatabase(databaseName);

// Đăng ký đối tượng IMongoDatabase vào DI container
builder.Services.AddSingleton<IMongoDatabase>(database);




var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Food}/{action=Index}/{id?}");

app.Run();
