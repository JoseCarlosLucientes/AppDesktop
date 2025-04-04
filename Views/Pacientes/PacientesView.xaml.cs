using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MedicalAppointments.ViewModels.Pacientes;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System.Windows.Controls;
using System;

namespace MedicalAppointments.Views.Pacientes
{
    public partial class PacientesView : Window
    {
        public PacientesView()
        {
            InitializeComponent();

            // Obtiene el PacienteService ya configurado en el contenedor DI
            var pacienteService = App.AppHost.Services.GetRequiredService<IPacienteService<PacienteDto>>();
            this.DataContext = new PacientesViewModel(pacienteService);

            // Pasa el servicio al ViewModel
            this.DataContext = new PacientesViewModel(pacienteService);

            // DEBUG: Verifica binding
            if (DataContext is PacientesViewModel vm)
            {
                vm.CargarPacientesCommand?.Execute(null); // Evita el NullReference
                Debug.WriteLine($"ViewModel cargado. Pacientes: {vm.Pacientes?.Count ?? 0}");
            }
        }

        private void NuevoPaciente_Click(object sender, RoutedEventArgs e)
        {
            var ventanaAlta = App.AppHost.Services.GetRequiredService<AltaPacienteView>();
            ventanaAlta.ShowDialog();
        }

        private void CargarPacientes()
        {
            if (DataContext is PacientesViewModel vm)
            {
                vm.CargarPacientesCommand.Execute(null);
            }
        }

        private void EditarPaciente_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is PacienteDto paciente)
            {
                var ventana = new AltaPacienteView(
                    App.AppHost.Services.GetRequiredService<IPacienteService<PacienteDto>>(),
                    paciente);
                ventana.ShowDialog();
                CargarPacientes(); // Recarga el listado tras editar
            }
        }

        private async void EliminarPaciente_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is PacienteDto paciente)
            {
                var resultado = MessageBox.Show($"¿Seguro que deseas eliminar al paciente '{paciente.Nombre} {paciente.Apellido1}'?",
                                                "Confirmar eliminación",
                                                MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        var service = App.AppHost.Services.GetRequiredService<IPacienteService<PacienteDto>>();
                        await service.DeletePacienteAsync(paciente.Id);

                        // Recargar la lista
                        CargarPacientes(); // Este método ya lo tienes
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar paciente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

      


    }
}
