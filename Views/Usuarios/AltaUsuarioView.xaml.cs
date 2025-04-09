using DocumentFormat.OpenXml.Wordprocessing;
using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.AppLogic.Services;
using MedicalAppointments.Domain.Entities;
using MedicalAppointments.Domain.Interfaces;
using MedicalAppointments.Infrastructure.Data;
using MedicalAppointments.ViewModels.Usuarios;
using System;
using System.Windows;

namespace MedicalAppointments.Views.Usuarios
{
    public partial class AltaUsuarioView : Window
    {
        private readonly IUserService<UserDto> _usuarioService;
        private readonly AppDbContext _context;
        private int? _usuarioId;
        private bool _modoEdicion = false;

        public AltaUsuarioView(IUserService<UserDto> usuarioService, AppDbContext context, UserDto usuarioExistente)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            _context = context;

            // Configurar según si es edición o alta
            if (usuarioExistente != null)
            {
                _modoEdicion = true;
                _usuarioId = usuarioExistente.Id;

                txtTitle.Text = "Editar usuario";
                btnGuardar.Content = "Guardar cambios";

                // Cargar datos del usuario en los campos si lo necesitas
                var viewModel = new AltaUsuarioViewModel(usuarioService, context)
                {
                    Usuario = usuarioExistente
                };
                DataContext = viewModel;
            }
            else
            {
                txtTitle.Text = "Nuevo usuario";
                btnGuardar.Content = "Crear usuario";

                DataContext = new AltaUsuarioViewModel(usuarioService, context);
            }
        }

        // Constructor para modo creación (sin parámetros)
        public AltaUsuarioView(IUserService<UserDto> service, AppDbContext context)
        {
            InitializeComponent();

            var viewModel = new AltaUsuarioViewModel(service, context)
            {
                EsEdicion = false,
                Usuario = new UserDto()
            };

            DataContext = viewModel;
        }
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataContext is AltaUsuarioViewModel vm)
                {
                    var usuarioDto = vm.Usuario;
                  

                    if (_modoEdicion)
                    {
                        await _usuarioService.UpdateUsuarioAsync(usuarioDto);
                        MessageBox.Show("Usuario actualizado correctamente");
                    }
                    else
                    {
                        await _usuarioService.CreateUsuarioAsync(usuarioDto);
                        MessageBox.Show("Usuario creado correctamente");
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar usuario: {ex.Message}");
            }
        }
    }
    
 
}