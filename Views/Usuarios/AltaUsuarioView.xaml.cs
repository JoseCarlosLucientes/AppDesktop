using System;
using System.Collections.Generic;
using System.Windows;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using MedicalAppointments.Domain.Entities;
using MedicalAppointments.Helpers;
using System.Windows.Controls;


namespace MedicalAppointments.Views.Usuarios
{
    public partial class AltaUsuarioView : Window
    {
        private readonly IUsuarioService<UsuarioDto> _usuarioService;

        public AltaUsuarioView(IUsuarioService<UsuarioDto> usuarioService)
        {
            InitializeComponent();
            InicializarFormulario();

            _usuarioService = usuarioService;            
            cbEspec.IsEnabled = false;
        }

        private void InicializarFormulario()
        {
            // chkActivo.IsChecked = true;
            CargarCombos();

        }

        private async void CrearUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtContrasena.Password) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellidos.Text) ||
                cbRol.SelectedItem == null)
            {
                MessageBox.Show("Por favor, rellena todos los campos obligatorios.", "Campos requeridos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var nuevoUsuarioDto = new UsuarioDto
                {
                    NombreUsuario = txtUsuario.Text.Trim(),
                    Contrasenia = txtContrasena.Password.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellidos = txtApellidos.Text.Trim(),
                    Dni = txtDni.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telf = txtTelefono.Text.Trim(),
                    Activo = chkActivo.IsChecked ?? true,
                    FechaCreacion = DateTime.Now,
                    IdRol = (int)cbRol.SelectedValue,
                    IdEspecialidad = (int)cbEspec.SelectedValue
                };

                await _usuarioService.CreateUserAsync(nuevoUsuarioDto);
                MessageBox.Show("Usuario creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CargarCombos()
        {
            try
            {
                // Obtener roles desde la base de datos
                var roles = DatabaseManager.ObtenerRoles();
                roles.Insert(0, new Rol { Id = 0, NombreRol = "Selecciona un rol..." });

                cbRol.ItemsSource = roles;
                cbRol.DisplayMemberPath = "NombreRol";
                cbRol.SelectedValuePath = "Id";
                cbRol.SelectedIndex = 0;
                cbRol.SelectionChanged += CbRol_SelectionChanged;

               

                // Obtener especialidades desde la base de datos
                var especialidades = DatabaseManager.ObtenerEspecialidades();
                especialidades.Insert(0, new Especialidad { Id = 0, NombreEspecialidad = "Selecciona especialidad..." });

                cbEspec.ItemsSource = especialidades;
                cbEspec.DisplayMemberPath = "NombreEspecialidad";
                cbEspec.SelectedValuePath = "Id";
                cbEspec.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CbRol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRol.SelectedItem is Rol rolSeleccionado)
            {
                if (rolSeleccionado.NombreRol.Equals("Especialista", StringComparison.OrdinalIgnoreCase))
                {
                    cbEspec.IsEnabled = true;
                }
                else
                {
                    cbEspec.IsEnabled = false;

                    // Buscar y asignar automáticamente la especialidad "Sin Especialidad"
                    foreach (var item in cbEspec.Items)
                    {
                        if (item is Especialidad esp && esp.NombreEspecialidad.Equals("Sin Especialidad", StringComparison.OrdinalIgnoreCase))
                        {
                            cbEspec.SelectedItem = esp;
                            break;
                        }
                    }
                }
            }
        }

    }
}
