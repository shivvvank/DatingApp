//this class entry point to our application
using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args); // creates web apps instance

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);


var app = builder.Build();//Add services before builder

//when we hit endpoint like weatherforecast it comes here 
// Configure the HTTP request pipeline.
// Is request made by authroised user?
app.MapControllers(); //middleware to map controller endpoints
app.UseCors(policyBuilder=>policyBuilder.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // asks Do you have valid token
app.UseAuthorization(); // asks what access you have 
app.Run();
