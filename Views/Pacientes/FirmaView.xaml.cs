using System.IO;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media.Imaging;

namespace MedicalAppointments.Views.Pacientes
{
    public partial class FirmaView : Window
    {
        public byte[]? FirmaBytes { get; private set; }

        public FirmaView()
        {
            InitializeComponent();
        }

        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            canvasFirma.Strokes.Clear();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (canvasFirma.Strokes.Count == 0)
            {
                MessageBox.Show("Debe realizar una firma antes de guardar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var stream = new MemoryStream();
            var rtb = new RenderTargetBitmap((int)canvasFirma.ActualWidth, (int)canvasFirma.ActualHeight, 96, 96, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvasFirma);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            FirmaBytes = stream.ToArray();

            DialogResult = true;
            Close();
        }
    }
}
