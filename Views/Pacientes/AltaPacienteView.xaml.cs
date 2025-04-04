using MedicalAppointments.AppLogic.DTOs;
using MedicalAppointments.Domain.Interfaces;
using System;
using System.Windows;


namespace MedicalAppointments.Views.Pacientes
{
    public partial class AltaPacienteView : Window
    {
        private readonly IPacienteService<PacienteDto> _pacienteService;
        private byte[]? _firmaPaciente;
        private readonly bool _modoEdicion;
        private readonly int? _pacienteId = null;       
        private readonly PacienteDto? _pacienteEditar;


        public AltaPacienteView(IPacienteService<PacienteDto> pacienteService, PacienteDto? paciente = null)
        {
            InitializeComponent();
            _pacienteService = pacienteService;
            InicializarControles();

            if (paciente != null)
            {
                _modoEdicion = true;
                _pacienteId = paciente.Id;
                txtTitle.Text = "Editar paciente"; // Cambiar título
                btnGuardar.Content = "Guardar cambios"; // Cambiar texto botón               
                CargarDatosPaciente(paciente);
            }
            else
            {
                txtTitle.Text = "Nuevo paciente";
                btnGuardar.Content = "Crear paciente";
            }
        }

        private void CargarDatosPaciente(PacienteDto p)
        {
            txtNombre.Text = p.Nombre;
            txtApellido1.Text = p.Apellido1;
            txtDNI.Text = p.Dni;
            dpFechaNacimiento.SelectedDate = p.FechaNacimiento;
            cbSexo.Text = p.Sexo;
            cbProfesion.Text = p.Profesion;
            txtDireccion.Text = p.Direccion;
            txtCodigoPostal.Text = p.CodigoPostal;
            txtProvincia.Text = p.Provincia;
            txtCiudad.Text = p.Ciudad;
            txtEmail.Text = p.Email;
            txtTelefonoFijo.Text = p.TelFijo;
            txtTelefonoMovil.Text = p.TelMovil;
            txtNumeroHistoria.Text = p.NumeroHistoriaClinica;
            chkEmail.IsChecked = p.PermiteEmail;
            chkSMS.IsChecked = p.PermiteSMS;
            chkWhatsApp.IsChecked = p.PermiteWhatsApp;
            chkMarketing.IsChecked = p.PermiteMarketing;
            txtOtros.Text = p.Observ;
            _firmaPaciente = p.Firma;
            chkRGPD.IsChecked = p.RGPD;
        }


        private void InicializarControles()
        {
            // Establecer valores por defecto vacíos o iniciales
            cbSexo.SelectedIndex = 0;
            cbProfesion.SelectedIndex = 0;
            dpFechaNacimiento.SelectedDate = null;

            // Desmarcar todos los checkboxes
            chkEmail.IsChecked = false;
            chkSMS.IsChecked = false;
            chkWhatsApp.IsChecked = false;
            chkMarketing.IsChecked = false;
        }

        private async void CrearPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCamposObligatorios())
                return;

            try
            {
                var pacienteDto = new PacienteDto
                {
                    Id = _pacienteId ?? 0, // Importante para el modo edición
                    Nombre = txtNombre.Text.Trim(),
                    Apellido1 = txtApellido1.Text.Trim(),
                    Dni = txtDNI.Text.Trim(),
                    FechaNacimiento = dpFechaNacimiento.SelectedDate ?? DateTime.Now,
                    Sexo = cbSexo.Text,
                    Profesion = cbProfesion.Text,
                    Direccion = txtDireccion.Text.Trim(),
                    CodigoPostal = txtCodigoPostal.Text.Trim(),
                    Provincia = txtProvincia.Text.Trim(),
                    Ciudad = txtCiudad.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    TelFijo = txtTelefonoFijo.Text.Trim(),
                    TelMovil = txtTelefonoMovil.Text.Trim(),
                    NumeroHistoriaClinica = txtNumeroHistoria.Text.Trim(),
                    PermiteEmail = chkEmail.IsChecked ?? false,
                    PermiteSMS = chkSMS.IsChecked ?? false,
                    PermiteWhatsApp = chkWhatsApp.IsChecked ?? false,
                    PermiteMarketing = chkMarketing.IsChecked ?? false,
                    Observ = txtOtros.Text?.Trim(),
                    Firma = _firmaPaciente,
                    RGPD = chkRGPD.IsChecked ?? false
                };

                if (_modoEdicion)
                {
                    await _pacienteService.UpdatePacienteAsync(pacienteDto); // Actualiza paciente
                    MessageBox.Show("Paciente actualizado correctamente", "Actualizado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _pacienteService.CreatePacienteAsync(pacienteDto); // Nuevo paciente
                    MessageBox.Show("Paciente creado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar paciente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarCamposObligatorios()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarError("El campo Nombre es obligatorio");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido1.Text))
            {
                MostrarError("El campo Apellidos es obligatorio");
                txtApellido1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MostrarError("El campo DNI es obligatorio");
                txtDNI.Focus();
                return false;
            }

            if (dpFechaNacimiento.SelectedDate == null)
            {
                MostrarError("Debe seleccionar una fecha de nacimiento");
                dpFechaNacimiento.Focus();
                return false;
            }

            return true;
        }

        private static void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

        private void RegistrarFirma_Click(object sender, RoutedEventArgs e)
        {
            var ventanaFirma = new FirmaView();
            if (ventanaFirma.ShowDialog() == true)
            {
                _firmaPaciente = ventanaFirma.FirmaBytes;
                MessageBox.Show("Firma registrada correctamente.", "Firma", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }      


    }
}