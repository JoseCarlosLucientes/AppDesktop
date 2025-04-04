using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalAppointments.ViewModels.Pacientes
{
    public class PacientesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PacienteDto> _pacientes;
        public ObservableCollection<PacienteDto> Pacientes
        {
            get => _pacientes;
            set
            {
                _pacientes = value;
                OnPropertyChanged(nameof(Pacientes));
            }
        }

        private readonly IPacienteService<PacienteDto> _service;
        public IRelayCommand CargarPacientesCommand { get; }


        public PacientesViewModel(IPacienteService<PacienteDto> service)
        {
            _service = service;
            _pacientes = new ObservableCollection<PacienteDto>();

            // Inicializa el comando y carga pacientes al iniciar
            CargarPacientesCommand = new RelayCommand(async () => await LoadPacientes());
            CargarPacientesCommand.Execute(null);
        }

        private async Task LoadPacientes()
        {
            var pacientes = await _service.GetAllPacientesAsync();

            // Evita romper el binding
            _pacientes.Clear();

            foreach (var p in pacientes)
                _pacientes.Add(p);
        }


        public ICommand AddPacienteCommand => new RelayCommand(async () =>
        {
            try
            {
                var nuevoPaciente = new PacienteDto
                {
                    // Inicializa propiedades
                };

                await _service.CreatePacienteAsync(nuevoPaciente);
                LoadPacientes(); // Refrescar datos

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
