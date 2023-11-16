using BlazorPagination.Shared;
using Database.ADO;
using Database.ADO.ValueObjects;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseOptions>(options => builder.Configuration.GetSection(DatabaseOptions.SectionName).Bind(options));
builder.Services.AddScoped<DataBaseWithADO>();
builder.Services.AddScoped<LazyAssemblyLoader>();

builder.Services.AddRazorPages();
builder.Services.AddSingleton<HttpClient>(sp =>
        {
            // Get the address that the app is currently running at
            var server = sp.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            string baseAddress = addressFeature.Addresses.First();
            return new HttpClient { BaseAddress = new Uri(baseAddress) };
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.MapGet("/virtualize-object", (DataBaseWithADO database) =>
{
    List<Customers> list = database.List<Customers>();
    return Results.Ok(list.Skip(101));
})
.WithName("Virtualize-object")
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

