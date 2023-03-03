using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using WebAPI.IServices;
using WebAPI.Services;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Middleware;
using Microsoft.OpenApi.Models;
using WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration;
var WebAPIFromEnv = Environment.GetEnvironmentVariable("API_SERVER");
var DB_FromEnv = Environment.GetEnvironmentVariable("DB_SERVER");
var API_Key_FromEnv = Environment.GetEnvironmentVariable("API_KEY");

//新加入用來Host API
//builder.Services.AddMvc(setupAction: option => option.EnableEndpointRouting = false)
//    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
builder.Services.AddControllers();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// 註冊 ICustomerService
builder.Services.AddSingleton<ICustomerService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    return new CustomerService(configuration, DB_FromEnv);

});



// 注入API Key驗證中間件
builder.Services.AddSingleton<ApiKeyMiddleware>(new ApiKeyMiddleware(null, API_Key_FromEnv ?? config.GetValue<string>("API_KEY")));

builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    var url = WebAPIFromEnv ?? config.GetValue<string>("BaseUrl");
    client.BaseAddress = new Uri(url);
});


builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    //指定project xml setting
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Description = "API Key Authentication"
    });

    c.OperationFilter<ApiKeyHeaderOperationFilter>();
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



app.UseMiddleware<ApiKeyMiddleware>(API_Key_FromEnv ?? config.GetValue<string>("API_KEY"));

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
