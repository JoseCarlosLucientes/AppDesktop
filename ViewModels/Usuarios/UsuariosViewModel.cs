using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalAppointments.ViewModels.Usuarios
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<UserDto> _usuarios;
        public ObservableCollection<UserDto> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));
            }
        }

        private readonly IUserService<UserDto> _service;
        public IRelayCommand CargarUsuariosCommand { get; }


        public UsuariosViewModel(IUserService<UserDto> service)
        {
            _service = service;
            _usuarios = new ObservableCollection<UserDto>();

            // Inicializa el comando y carga pacientes al iniciar
            CargarUsuariosCommand = new RelayCommand(async () => await LoadUsuarios());
            CargarUsuariosCommand.Execute(null);
        }

        private async Task LoadUsuarios()
        {
            var usuarios = await _service.GetAllUsuariosAsync();

            // Evita romper el binding
            _usuarios.Clear();

            foreach (var p in usuarios)
                _usuarios.Add(p);
        }


        public ICommand AddUserCommand => new RelayCommand(async () =>
        {
            try
            {
                var nuevoUsuario = new UserDto
                {
                    // Inicializa propiedades
                };

                await _service.CreateUsuarioAsync(nuevoUsuario);
                LoadUsuarios(); // Refrescar datos

                // O alternativamente para mejor performance:
                // var pacienteCreado = await _service.CreateAndReturnPacienteAsync(nuevoPaciente);
                // _pacientes.Add(pacienteCreado);
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        });

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}