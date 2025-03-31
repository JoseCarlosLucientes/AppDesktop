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
using Microsoft.Extensions.Logging;
using System;
using MedicalAppointments.ViewModels.Pacientes;
using MedicalAppointments.Views.Pacientes;

namespace MedicalAppointments
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); //  Evita el EventLog
                    logging.AddDebug();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // DbContext Factory
                    services.AddDbContextFactory<AppDbContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                    // Repositorios
                    services.AddScoped<IPacienteRepository, PacienteRepository>();

                    // Servicios
                    services.AddScoped<IPacienteService<PacienteDto>, PacienteService>();
                    services.AddScoped<PacienteService>();

                    // AutoMapper
                    services.AddAutoMapper(typeof(MappingProfile));

                    // Vista principal
                    services.AddSingleton<MenuPrincipal>();
                    services.AddTransient<PacientesViewModel>();
                    services.AddTransient<PacientesView>();

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