using System.Windows;
using MedicalAppointments.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MedicalAppointments.ViewModels.Pacientes;
using MedicalAppointments.AppLogic.Services;

namespace MedicalAppointments.Views.Pacientes
{
    public partial class PacientesView : Window
    {
        public PacientesView()
        {
            InitializeComponent();

            // Obtiene el PacienteService ya configurado en el contenedor DI
            var pacienteService = App.AppHost.Services.GetRequiredService<PacienteService>();

            // Pasa el servicio al ViewModel
            this.DataContext = new PacientesViewModel(pacienteService);

            // DEBUG: Verifica binding
            if (DataContext is PacientesViewModel vm)
            {
                Debug.WriteLine($"ViewModel cargado. Pacientes: {vm.Pacientes?.Count ?? 0}");
            }
        }

        private void NuevoPaciente_Click(object sender, RoutedEventArgs e)
        {
            var dbContext = App.AppHost.Services.GetService(typeof(AppDbContext)) as AppDbContext;
            var ventanaAlta = new AltaPacienteView();
            ventanaAlta.ShowDialog();
        }
    }
}
