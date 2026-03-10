using Microsoft.AspNetCore.Components.Authorization;
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

// Auth state and provider
builder.Services.AddScoped<IAuthStateService, AuthStateService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();

// HTTP token handler
builder.Services.AddTransient<AuthTokenHandler>();

// Resolve API base URL once — fail fast if not configured
var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException(
        "ApiBaseUrl is not configured. Add it to appsettings.json.");

var apiBaseUri = new Uri(apiBaseUrl);

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(
    client => client.BaseAddress = apiBaseUri)
    .AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddHttpClient<IWalletApiClient, WalletApiClient>(
    client => client.BaseAddress = apiBaseUri)
    .AddHttpMessageHandler<AuthTokenHandler>();

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