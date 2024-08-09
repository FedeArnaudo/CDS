using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CDS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(StatusForm_Load);
        }
        private void StatusForm_Load(object sender, EventArgs e)
        {
            if (!Configuration.ExisteConfiguracion())
            {
                InitialSettings initialSettings = new InitialSettings
                {
                    Owner = this
                };
                initialSettings.Closed += VerConfig_Closed;
                initialSettings.Show();
                Hide();
                IsEnabled = false;
            }
            else
            {
                Init();
            }
        }
        private void VerConfig_Closed(object sender, EventArgs e)
        {
            IsEnabled = true;
            Show();
            _ = Activate();
            Init();
        }
        private void Init()
        {
            Info infoConfig = Configuration.LeerConfiguracion();
            if (infoConfig != null)
            {
                _ = MessageBox.Show("Programa iniciado correctamente");
            }
            else
            {
                //_ = Log.Instance.WriteLog("Error en la lectura del archivo inicial", Log.LogType.t_info);
                Console.WriteLine("Error en la lectura del archivo inicial");
                Close();
            }
        }

        #region EVENTOS
        private void BtnCambiarConfig_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVerConfigEstacion_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVerDespachos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVerTanques_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVerProductos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCierre_Anterior_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion
    }
}
