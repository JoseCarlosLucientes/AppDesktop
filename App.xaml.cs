using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MedicalAppointments.Domain.Interfaces;
using MedicalAppointments.Infrastructure.Repositories;
using MedicalAppointments.AppLogic.Services;
using System.IO;
using System.Windows;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.AppLogic.Mapping;
using MedicalAppointments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MedicalAppointments.Views;

namespace MedicalAppointments
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Configuración de DbContext Factory
                    services.AddDbContextFactory<AppDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    // Registro de repositorios
                    services.AddScoped<IPacienteRepository, PacienteRepository>();

                    // Registro de servicios
                    services.AddScoped<IPacienteService<PacienteDto>, PacienteService>();

                    // Registro de AutoMapper
                    services.AddAutoMapper(typeof(MappingProfile)); // Asumiendo que tienes MappingProfile en AppLogic

                    // Registro de ventana principal
                    services.AddSingleton<MenuPrincipal>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MenuPrincipal>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}