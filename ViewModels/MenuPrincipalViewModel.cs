using CommunityToolkit.Mvvm.Input;
using MedicalAppointments.Views;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalAppointments.ViewModels
{
    public class MainViewModel
    {
        public ICommand OpenPacientesCommand { get; }

        public MainViewModel()
        {
            OpenPacientesCommand = new RelayCommand(OpenPacientes);
        }

        private void OpenPacientes()
        {
            // Abre la ventana de pacientes
            var pacientesView = new Views.Pacientes.PacientesView();
            pacientesView.Show();

            // Opcional: Cierra el menú principal si es necesario
            Application.Current.Windows.OfType<MenuPrincipal>().FirstOrDefault()?.Close();
        }
    }
}