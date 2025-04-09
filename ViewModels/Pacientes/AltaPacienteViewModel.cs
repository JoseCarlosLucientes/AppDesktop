using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using System.ComponentModel;
using System.Windows.Input;
using System;
using MedicalAppointments.Domain.Interfaces;



namespace MedicalAppointments.ViewModels.Pacientes
{
    public class AltaPacienteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IPacienteService<PacienteDto> _pacienteService;
        public PacienteDto NuevoPaciente { get; set; }
        public ICommand GuardarCommand { get; }



        public AltaPacienteViewModel(IPacienteService<PacienteDto> pacienteService)
        {

            _pacienteService = pacienteService;
            NuevoPaciente = new PacienteDto();
            GuardarCommand = new RelayCommand(GuardarPaciente, CanGuardarPaciente);
        }

        private async void GuardarPaciente()
        {
            try
            {
                await _pacienteService.CreatePacienteAsync(NuevoPaciente);
                // Notificar éxito y cerrar la ventana o limpiar el formulario
            }
            catch (Exception ex)
            {
                // Manejar errores y notificar al usuario
            }
        }

        private bool CanGuardarPaciente()
        {
            // Validar  campos obligatorios completos)
            return !string.IsNullOrWhiteSpace(NuevoPaciente.Nombre) &&
                   !string.IsNullOrWhiteSpace(NuevoPaciente.Apellido1) &&
                   !string.IsNullOrWhiteSpace(NuevoPaciente.Dni);
                   
        }

        // Implementación de INotifyPropertyChanged...
    }
}