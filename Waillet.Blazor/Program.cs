using Microsoft.FluentUI.AspNetCore.Components;
using Waillet.Blazor.Components;
using Waillet.Blazor.Services.Auth;
using Waillet.Blazor.Services.Wallet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Fluent UI components services
builder.Services.AddFluentUIComponents();

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(string.IsNullOrWhiteSpace(apiBaseUrl)
        ? "https://localhost:7005/"
        : apiBaseUrl);
});

builder.Services.AddHttpClient<IWalletApiClient, WalletApiClient>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(string.IsNullOrWhiteSpace(apiBaseUrl)
        ? "https://localhost:7005/"
        : apiBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();