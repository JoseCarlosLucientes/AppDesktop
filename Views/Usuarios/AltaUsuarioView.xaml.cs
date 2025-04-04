using MedicalAppointments.ViewModels.Usuarios;
using System.Windows;

namespace MedicalAppointments.Views.Usuarios
{
    public partial class AltaUsuarioView : Window
    {
        public AltaUsuarioView(AltaUsuarioViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}