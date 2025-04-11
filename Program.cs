using GameStore.Services;
// using Azure.Identity;
// using Azure.Security.KeyVault.Secrets;


var builder = WebApplication.CreateBuilder(args);

// var keyVaultUrl = "https://PM.vault.azure.net/";

//builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Lägg till tjänster för MVC och GameService
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<GameService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new GameService(config);
});

var app = builder.Build();


// Konfigurera HTTP-anrop
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Games}/{action=Index}/{id?}");

app.Run();
