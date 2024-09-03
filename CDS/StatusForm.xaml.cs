 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MessageBox = System.Windows.MessageBox;

namespace CDS
{
    /// <summary>
    /// Interaction logic for StatusForm.xaml
    /// </summary>
    public partial class StatusForm : Window
    {
        private NotifyIcon notifyIcon;
        public StatusForm()
        {
            BuscarPrograma("PumpController");
            InitializeComponent();
            Icon = new BitmapImage(new Uri("pack://application:,,,/CDS;component/Images/LogoSurtidor.ico"));
            SetupNotifyIcon();
            Loaded += new RoutedEventHandler(StatusForm_Load);
        }
        private void StatusForm_Load(object sender, EventArgs e)
        {
            if (!Configuration.ExisteConfiguracion())
            {
                Views.InitialSettings initialSettings = new Views.InitialSettings
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
        private void Init()
        {
            if (Configuration.LeerConfiguracion() != null && Controlador.Init(Configuration.LeerConfiguracion().TipoDeControlador))
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
        private void VerConfig_Closed(object sender, EventArgs e)
        {
            IsEnabled = true;
            Show();
            _ = Activate();
            Init();
        }
        private void BuscarPrograma(string prog)
        {
            Process[] pname = Process.GetProcessesByName(prog);
            if (pname.Length > 1)
            {
                _ = MessageBox.Show("El programa ya esta abierto");
                Process[] procesos = Process.GetProcessesByName(prog);
                procesos[0].Kill();
            }
        }
        #region PROCESO EN SEGUNDO PLANO
        private void SetupNotifyIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = new Icon("LogoSurtidor.ico"),
                Visible = false,
                Text = "Controlador De Surtidores"
            };

            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            ContextMenu contextMenu = new ContextMenu();
            _ = contextMenu.MenuItems.Add("Restaurar", (s, e) => RestoreFromTray());
            _ = contextMenu.MenuItems.Add("Salir", (s, e) => ExitApplication());

            notifyIcon.ContextMenu = contextMenu;
        }
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }
        private void RestoreFromTray()
        {
            Show();
            WindowState = WindowState.Normal;
            notifyIcon.Visible = false;
        }
        private void ExitApplication()
        {
            notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            notifyIcon.Dispose();
            base.OnClosed(e);
        }
        #endregion
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
