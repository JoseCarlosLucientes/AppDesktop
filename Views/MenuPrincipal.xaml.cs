using MedicalAppointments.ViewModels.Pacientes;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace MedicalAppointments.Views
{ 
    /// <summary>
    /// Ventana principal de la aplicación. Configura el DataContext con el ViewModel.
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        /// <summary>
        /// Constructor que resuelve el ViewModel desde el contenedor de dependencias.
        /// </summary>
        public MenuPrincipal()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<PacientesViewModel>(); // Enlaza ViewModel
        }

        /// <summary>
        /// Permite arrastrar la ventana al hacer clic y arrastrar en la barra superior
        /// </summary>
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove(); // Método que mueve la ventana
            }
        }

        // Minimiza la ventana
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Cierra la ventana
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            // Opcional: Cierra toda la aplicación si esta es la ventana principal
            // Application.Current.Shutdown(); 
        }
    }
}