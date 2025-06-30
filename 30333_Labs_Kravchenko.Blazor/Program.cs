using _30333_Labs_Kravchenko.Blazor.Components;
using _30333_Labs_Kravchenko.Blazor.Services;
using _30333_Labs_Kravchenko.Domain.Entities;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient<IProductService<Medication>, ApiProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7002/api/medications/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();