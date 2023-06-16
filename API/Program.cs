//this class entry point to our application
using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // creates web apps instance

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => //convention
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
var app = builder.Build();//Add services before builder

//when we hit endpoint like weatherforecast it comes here 
// Configure the HTTP request pipeline.
// Is request made by authroised user?
app.MapControllers(); //middleware to map controller endpoints
app.UseCors(policyBuilder=>policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.Run();
