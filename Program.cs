using Microsoft.EntityFrameworkCore;
using WebApp.models;


var builder = WebApplication.CreateBuilder(args);
string connection = "Server=(localdb)\\mssqllocaldb;Database=applicationdb;Trusted_Connection=True;";
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());
app.MapGet("/api/cars", async (ApplicationContext db) => await db.Cars.ToListAsync());

app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;

});

app.MapPost("/api/cars", async (Car car, ApplicationContext db) =>
{
    await db.Cars.AddAsync(car);
    await db.SaveChangesAsync();
    return car;

});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);
    
    if (user == null) return Results.NotFound(new {message = "Клиент не найден"});
    user.Age = userData.Age;
    user.Name = userData.Name;
    await db.SaveChangesAsync();
    return Results.Json(user);

});

app.MapPut("/api/cars", async (Car carData, ApplicationContext db) =>
{
    var car = await db.Cars.FirstOrDefaultAsync(u => u.Id == carData.Id);
    
    if (car == null) return Results.NotFound(new {message = "Машина не найдена"});
    car.Marka = carData.Marka;
    car.Model = carData.Model;
    car.Number = carData.Number;
    await db.SaveChangesAsync();
    return Results.Json(car);

});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
    
    if (user == null) return Results.NotFound(new {message = "Пользователь не найден"});

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapDelete("/api/cars/{id:int}", async (int id, ApplicationContext db) =>
{
    Car? car = await db.Cars.FirstOrDefaultAsync(u => u.Id == id);
    
    if (car == null) return Results.NotFound(new {message = "Машина не найдена"});

    db.Cars.Remove(car);
    await db.SaveChangesAsync();
    return Results.Json(car);
});


app.Run();