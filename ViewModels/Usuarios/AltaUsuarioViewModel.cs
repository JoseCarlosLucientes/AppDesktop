using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using MedicalAppointments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedicalAppointments.ViewModels.Usuarios
{
    public class AltaUsuarioViewModel : ObservableObject
    {
        private readonly IUserService<UserDto> _usuarioService;

        public UserDto Usuario { get; set; } = new();
        public bool EsEdicion { get; set; } = false;

        public ObservableCollection<RolDto> Roles { get; set; } = new();
        public ObservableCollection<EspecialidadDto> Especialidades { get; set; } = new();
        public ObservableCollection<EspecialidadDto> EspecialidadesVisibles { get; set; } = new();


        private RolDto? _rolSeleccionado;
        public RolDto? RolSeleccionado
        {
            get => _rolSeleccionado;
            set
            {
                SetProperty(ref _rolSeleccionado, value);
                OnPropertyChanged(nameof(EspecialidadHabilitada));
                FiltrarEspecialidades();
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

        private readonly AppDbContext _context;

        public AltaUsuarioViewModel(IUserService<UserDto> usuarioService, AppDbContext context)
        {
            _usuarioService = usuarioService;
            _context = context;

            GuardarCommand = new AsyncRelayCommand(GuardarUsuarioAsync/*, CanGuardar*/);
            CancelarCommand = new RelayCommand(Cancelar);

            _ = CargarCombosAsync();
        }

        private async Task CargarCombosAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                Roles.Clear();
                foreach (var rol in roles)
                    Roles.Add(new RolDto { Id = rol.Id, NombreRol = rol.NombreRol });

                var especialidades = await _context.Especialidades.ToListAsync();
                Especialidades.Clear();
                foreach (var especialidad in especialidades)
                    Especialidades.Add(new EspecialidadDto { Id = especialidad.Id, NombreEspecialidad = especialidad.NombreEspecialidad });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            FiltrarEspecialidades();
        }       
        

        private async Task GuardarUsuarioAsync()
        {
            try
            {
                Usuario.IdRol = RolSeleccionado?.Id ?? 0;
                Usuario.IdEspecialidad = EspecialidadSeleccionada?.Id ?? 1; // Asumiendo 1 es "Sin Especialidad"

                await _usuarioService.CreateUsuarioAsync(Usuario);

                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

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

        private void FiltrarEspecialidades()
        {
            EspecialidadesVisibles.Clear();

            if (RolSeleccionado?.NombreRol?.ToLower() == "especialista")
            {
                // Mostrar todas las especialidades
                foreach (var esp in Especialidades)
                    EspecialidadesVisibles.Add(esp);
            }
            else
            {
                // Mostrar solo la opción "Sin especialidad"
                var sinEsp = Especialidades.FirstOrDefault(e => e.NombreEspecialidad == "Sin especialidad");
                if (sinEsp != null)
                {
                    EspecialidadesVisibles.Add(sinEsp);
                    EspecialidadSeleccionada = sinEsp;
                }
            }

            OnPropertyChanged(nameof(EspecialidadesVisibles));
            OnPropertyChanged(nameof(EspecialidadSeleccionada));
        }

    }
}
