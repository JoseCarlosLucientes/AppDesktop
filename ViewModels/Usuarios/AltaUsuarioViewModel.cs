using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using System.ComponentModel;
using System.Windows.Input;
using System;
using MedicalAppointments.Domain.Interfaces;



namespace MedicalAppointments.ViewModels.Usuarios
{
    public class AltaUsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IUsuarioService<UsuarioDto> _usuaraioService;
        public UsuarioDto NuevoUsuario { get; set; }
        public ICommand GuardarCommand { get; }



        public AltaUsuarioViewModel(IUsuarioService<UsuarioDto> usuarioService)
        {

            _usuaraioService = usuarioService;
            NuevoUsuario = new UsuarioDto();
            GuardarCommand = new RelayCommand(GuardarUsuario, CanGuardarUsuario);
        }

        private async void GuardarUsuario()
        {
            try
            {
                await _usuaraioService.CreateUsuarioAsync(NuevoUsuario);
                // Notificar éxito y cerrar la ventana o limpiar el formulario
            }
            catch (Exception ex)
            {
                // Manejar errores y notificar al usuario
            }
        }

        private bool CanGuardarUsuario()
        {
            // Validar si se pueden guardar los datos (por ejemplo, campos obligatorios completos)
            return !string.IsNullOrWhiteSpace(NuevoUsuario.Nombre) &&
                   !string.IsNullOrWhiteSpace(NuevoUsuario.Apellidos) &&
                   !string.IsNullOrWhiteSpace(NuevoUsuario.Dni);
        }

        // Implementación de INotifyPropertyChanged...
    }
}