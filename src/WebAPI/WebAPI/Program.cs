using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.IServices;
using WebAPI.Services;
using System.Reflection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration;

//新加入用來Host API
builder.Services.AddMvc(setupAction: option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(config.GetValue<string>("BaseUrl"));
});


builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    //指定project xml setting
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API For CRUD V1");


});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//新加入用來Host API
app.UseMvcWithDefaultRoute();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
