using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedicalAppointments.ViewModels.Usuarios
{
    public class AltaUsuarioViewModel : ObservableObject
    {
        private readonly IUsuarioService<UsuarioDto> _usuarioService;

        public UsuarioDto Usuario { get; set; } = new();
        public ObservableCollection<RolDto> Roles { get; set; } = new();
        public ObservableCollection<EspecialidadDto> Especialidades { get; set; } = new();

        private RolDto? _rolSeleccionado;
        public RolDto? RolSeleccionado
        {
            get => _rolSeleccionado;
            set
            {
                SetProperty(ref _rolSeleccionado, value);
                OnPropertyChanged(nameof(EspecialidadHabilitada));
                ActualizarEspecialidadPorDefecto();
            }
        }

        private EspecialidadDto? _especialidadSeleccionada;
        public EspecialidadDto? EspecialidadSeleccionada
        {
            get => _especialidadSeleccionada;
            set => SetProperty(ref _especialidadSeleccionada, value);
        }

        public bool EspecialidadHabilitada =>
            RolSeleccionado?.NombreRol?.ToLower() == "especialista";

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AltaUsuarioViewModel(IUsuarioService<UsuarioDto> usuarioService)
        {
            _usuarioService = usuarioService;

            GuardarCommand = new RelayCommand(async () => await GuardarUsuarioAsync(), CanGuardar);
            CancelarCommand = new RelayCommand(Cancelar);

            CargarCombosAsync();
        }

        private async Task CargarCombosAsync()
        {
            try
            {
                var roles = await _usuarioService.ObtenerRolesAsync();
                var especialidades = await _usuarioService.ObtenerEspecialidadesAsync();

                Roles = new ObservableCollection<RolDto>(roles);
                Especialidades = new ObservableCollection<EspecialidadDto>(especialidades);

                OnPropertyChanged(nameof(Roles));
                OnPropertyChanged(nameof(Especialidades));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarEspecialidadPorDefecto()
        {
            if (!EspecialidadHabilitada)
            {
                EspecialidadSeleccionada = Especialidades.FirstOrDefault(e => e.NombreEspecialidad == "Sin Especialidad");
            }
        }

        private bool CanGuardar()
        {
            return !string.IsNullOrWhiteSpace(Usuario.NombreUsuario)
                && !string.IsNullOrWhiteSpace(Usuario.Contrasenia)
                && !string.IsNullOrWhiteSpace(Usuario.Nombre)
                && !string.IsNullOrWhiteSpace(Usuario.Apellidos)
                && RolSeleccionado != null;
        }

        private async Task GuardarUsuarioAsync()
        {
            try
            {
                Usuario.IdRol = RolSeleccionado?.Id ?? 0;
                Usuario.IdEspecialidad = EspecialidadSeleccionada?.Id ?? 1; // "Sin Especialidad"

                await _usuarioService.CreateUserAsync(Usuario);

                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                // Cerrar ventana desde código-behind si es necesario
                Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)
                    ?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this)
                ?.Close();
        }
    }
}
