using Microsoft.AspNetCore.ResponseCompression;
using QuizExperiment.Admin.Server.Hubs;
using QuizExperiment.Admin.Server.Services;
using QuizExperiment.Models;
using QuizExperiment.Models.Client;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new PolymorphicQuestionConverter());
        options.JsonSerializerOptions.Converters.Add(new PolymorphicQuestionListConverter());
        // Optionally, keep property naming policy consistent
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IQuestionSetService>(r=> 
    new AzureStorageQuestionSetService(builder.Configuration));

IWebHostEnvironment environment = builder.Environment;
//var signalRServiceBuilder = builder.Services.AddSignalR()
//    .AddJsonProtocol(options=>
//    {
//        options.PayloadSerializerOptions.Converters.Add(new ClientQuestionConverter());
//    });
var signalRServiceBuilder = builder.Services.AddSignalR();

if(!environment.IsDevelopment())
{
    signalRServiceBuilder.AddAzureSignalR();
}


builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/"), second =>
{
    second.UseBlazorFrameworkFiles("/");
    second.UseStaticFiles();

    second.UseRouting();
    second.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("/{*path:nonfile}", "/index.html");
    });
});

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<QuizHub>("/quizhub");

app.Run();
