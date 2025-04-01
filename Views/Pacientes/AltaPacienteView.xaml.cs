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
        public AltaPacienteView(IPacienteService<PacienteDto> pacienteService)
        {
            InitializeComponent();
            _pacienteService = pacienteService;
            InicializarControles();
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
                    Nombre = txtNombre.Text.Trim(),
                    Apellido1 = txtApellido1.Text.Trim(),
                    // Apellido2 = txtApellido2.Text.Trim(),
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
                    //RGPD = chkRGPD.IsChecked ?? false
                };

                pacienteDto.Firma = _firmaPaciente;


                await _pacienteService.CreatePacienteAsync(pacienteDto);
                MessageBox.Show("Paciente guardado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Paciente duplicado", MessageBoxButton.OK, MessageBoxImage.Warning);
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