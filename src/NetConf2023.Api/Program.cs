using Database.ADO;
using Database.ADO.ValueObjects;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NetConf2023.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.Configure<DatabaseOptions>(options => builder.Configuration.GetSection(DatabaseOptions.SectionName).Bind(options));
builder.Services.AddScoped<DataBaseWithADO>();
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<HttpClient>(sp =>
        {
            // Get the address that the app is currently running at
            var server = sp.GetRequiredService<IServer>();
            var httpFactory = sp.GetRequiredService<IHttpClientFactory>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            string baseAddress = addressFeature.Addresses.First();
            var httpclient = httpFactory.CreateClient();
            httpclient.BaseAddress = new Uri(baseAddress);
            return httpclient;
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();


app.MapGet("/all", (DataBaseWithADO database) =>
{
    List<Customers> list = database.List<Customers>("vibno", 101, 50);
    return Results.Ok(list);
})
.WithName("All")
.WithOpenApi();

app.MapGet("/data/{page}/{elements}", (DataBaseWithADO database, int page, int elements) =>
{
    List<Customers> list = database.List<Customers>("vibno", page + 101, elements);
    return Results.Ok(list);
})
.WithName("Data")
.WithOpenApi();

app.MapGet("/virtual-object", (DataBaseWithADO database) =>
{
    List<Customers> list = database.List<Customers>();
    return Results.Ok(list.Skip(101));
})
.WithName("Virtual-object")
.WithOpenApi();
app.MapGet("/virtualize/{page}/{elements}", (DataBaseWithADO database, int page, int elements) =>
{
    List<Customers> list = database.List<Customers>("vibno", page, elements);
    return Results.Ok(list);
})
.WithName("Virtualize")
.WithOpenApi();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapRazorPages();
app.MapFallbackToPage("/_Host");

app.Run();

