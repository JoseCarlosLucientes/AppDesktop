using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using MedicalAppointments.ViewModels.Usuarios;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System.Windows.Controls;
using System;
using MedicalAppointments.Infrastructure.Data;

namespace MedicalAppointments.Views.Usuarios
{
    public partial class UsuariosView : Window
    {
        public UsuariosView()
        {
            InitializeComponent();

            // Obtiene el UserService ya configurado en el contenedor DI
            var usuarioService = App.AppHost.Services.GetRequiredService<IUserService<UserDto>>();
            this.DataContext = new UsuariosViewModel(usuarioService);

            // Pasa el servicio al ViewModel
            this.DataContext = new UsuariosViewModel(usuarioService);

            // DEBUG: Verifica binding
            if (DataContext is UsuariosViewModel vm)
            {
                vm.CargarUsuariosCommand?.Execute(null); // Evita el NullReference
                Debug.WriteLine($"ViewModel cargado. Usuarios: {vm.Usuarios?.Count ?? 0}");
            }
        }

        private void NuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            var ventanaAlta = App.AppHost.Services.GetRequiredService<AltaUsuarioView>();
            ventanaAlta.ShowDialog();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            if (DataContext is UsuariosViewModel vm)
            {
                vm.CargarUsuariosCommand.Execute(null);
            }
        }

        private void EditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is UserDto user)
            {
                var ventana = new AltaUsuarioView(
                    App.AppHost.Services.GetRequiredService<IUserService<UserDto>>(),
                    App.AppHost.Services.GetRequiredService<AppDbContext>(),
                    user);

                ventana.ShowDialog();

                // Recargar la lista al cerrar
                if (DataContext is UsuariosViewModel vm)
                    vm.CargarUsuariosCommand.Execute(null);
            }
        }
        
        private async void EliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is UserDto user)
            {
                var resultado = MessageBox.Show($"¿Seguro que deseas eliminar al usuario '{user.Nombre} {user.Apellidos}'?",
                                                "Confirmar eliminación",
                                                MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        var service = App.AppHost.Services.GetRequiredService<IUserService<UserDto>>();
                        await service.DeleteUsuarioAsync(user.Id);

                        // Recargar la lista
                        CargarUsuarios(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }




    }
}
