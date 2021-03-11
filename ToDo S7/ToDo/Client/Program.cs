using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;

namespace ToDo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddAntDesign();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"http://localhost:5500") });

            builder.Services.AddScoped<TaskDetailServices>();

            builder.Services.AddAuthorizationCore(option =>
            {
                option.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));

            });
            builder.Services.AddScoped<AuthenticationStateProvider, AuthProvider>();

            await builder.Build().RunAsync();
        }
    }
}
