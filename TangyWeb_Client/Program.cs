using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TangyWeb_Client;
using TangyWeb_Client.Service;
using TangyWeb_Client.Service.IService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// new uri by default is builder.HostEnvironment.Base address, but we want to call our API, so we change it to the value
// we set in wwwroot/appsettings.json:
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAPIUrl")) });
// now everytime we inject the http client we will point to our API by default

builder.Services.AddScoped<IProductService, ProductService>();

await builder.Build().RunAsync();
