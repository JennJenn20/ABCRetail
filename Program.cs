using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ABCRetailWebApplication.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Load connection string from appsettings.json
var storageConnectionString = builder.Configuration.GetConnectionString("storageConnectionString");

// Define your Azure resource names
var blobContainerName = "blobstorage"; 
var fileShareName = "fileshares";
var tableName = "Table"; 
var queueName = "queue"; // Change this to your actual queue name

// Register Azure services with the connection string and resource names
builder.Services.AddSingleton(new AzureBlobService(storageConnectionString, blobContainerName));
builder.Services.AddSingleton(new AzureFileService(storageConnectionString, fileShareName));
builder.Services.AddSingleton(new AzureTableService(storageConnectionString, tableName));
builder.Services.AddSingleton(new AzureQueueService(storageConnectionString, queueName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
