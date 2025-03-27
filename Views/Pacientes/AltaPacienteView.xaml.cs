using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Windows;

namespace MedicalAppointments.Views.Pacientes
{
    public partial class AltaPacienteView : Window
    {
        public AltaPacienteView()
        {
            InitializeComponent();
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

        private void CrearPaciente_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCamposObligatorios())
                return;

            try
            {
                var paciente = new
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellidos = txtApellidos.Text.Trim(),
                    DNI = txtDNI.Text.Trim(),
                    FechaNacimiento = dpFechaNacimiento.SelectedDate,
                    Sexo = cbSexo.Text,
                    Profesion = cbProfesion.Text,
                    Direccion = txtDireccion.Text.Trim(),
                    CodigoPostal = txtCodigoPostal.Text.Trim(),
                    Provincia = txtProvincia.Text.Trim(),
                    Ciudad = txtCiudad.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    TelefonoFijo = txtTelefonoFijo.Text.Trim(),
                    TelefonoMovil = txtTelefonoMovil.Text.Trim(),
                    NumeroHistoria = txtNumeroHistoria.Text.Trim(),
                    ContactoEmail = chkEmail.IsChecked,
                    ContactoSMS = chkSMS.IsChecked,
                    ContactoWhatsApp = chkWhatsApp.IsChecked,
                    ContactoMarketing = chkMarketing.IsChecked,
                    Otros = txtOtros.Text.Trim()
                };

                // Lógica para guardar el paciente
                MessageBox.Show("Paciente creado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el paciente: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                MostrarError("El campo Apellidos es obligatorio");
                txtApellidos.Focus();
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
    }
}