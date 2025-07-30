using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Planets.Models;
using Planets.Services;
using Planets.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Planets
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite("Data Source=planets.db"));

                    services.AddScoped<MainWindow>();
                    services.AddScoped<MainWindowViewModel>();
                    services.AddScoped<ListViewModel>();                    
                    services.AddScoped<DetailViewModel>();
                    services.AddScoped<FilterViewModel>();
                    services.AddScoped<PropertyViewModel>();
                    services.AddScoped<PlanetViewModel>();
                    services.AddSingleton<IDialogService, DialogService>();
                })
                .Build();

            AppHost.Start();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
