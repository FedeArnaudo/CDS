using System;
using System.Collections.Generic;
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
using CheckBox = System.Windows.Controls.CheckBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace CDS
{
    /// <summary>
    /// Interaction logic for CEMSettings.xaml
    /// </summary>
    public partial class SettingsCEM : Page
    {
        private readonly Dictionary<string, string> parametrosCEM = new Dictionary<string, string>();
        private string protocolo = "";
        public SettingsCEM()
        {
            InitializeComponent();
            parametrosCEM.Add("Ruta", "");
            parametrosCEM.Add("IP", "");
            parametrosCEM.Add("Protocolo", "");
        }

        private void BtnRutaPN_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer, // Carpeta raíz (opcional)
                Description = "Selecciona una carpeta" // Descripción del diálogo (opcional)
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                TextBoxPryNuevo.Text = folderPath;
            }
        }

        private void BtnInfoIP_Click(object sender, RoutedEventArgs e)
        {
            string info = $"La IP se debe consultar con la Estación\n";
            _ = MessageBox.Show(info);
        }

        private void BtnInfoProtocolo_Click(object sender, RoutedEventArgs e)
        {
            string info = $"El protocolo indica la cantidad maxima\n" +
                          $"de surtidores que puede tener la estacion.\n" +
                          $"Iniciar con 16, en su defecto, con 32.";
            _ = MessageBox.Show(info);
        }

        private void CheckBoxProtocol16_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxProtocol16 = (CheckBox)sender;
            CheckBoxProtocol16.IsChecked = true;
            protocolo = "16";
            CheckBoxProtocol32.IsChecked = false;
        }

        private void CheckBoxProtocol32_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxProtocol32 = (CheckBox)sender;
            CheckBoxProtocol32.IsChecked = true;
            protocolo = "32";
            CheckBoxProtocol16.IsChecked = false;
        }

        private void CheckBoxProtocol16_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxProtocol16 = (CheckBox)sender;
            CheckBoxProtocol16.IsChecked = false;
            CheckBoxProtocol32.IsChecked = true;
            protocolo = "32";
        }

        private void CheckBoxProtocol32_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxProtocol32 = (CheckBox)sender;
            CheckBoxProtocol32.IsChecked = false;
            CheckBoxProtocol16.IsChecked = true;
            protocolo = "16";
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Combinación de teclas Ctrl + Shift + E
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (e.Key == Key.E)
                    {
                        ComboBoxMode.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        public Dictionary<string, string> GetConfiguration()
        {
            if (parametrosCEM["Ruta"] != TextBoxPryNuevo.Text)
            {
                parametrosCEM["Ruta"] = TextBoxPryNuevo.Text;
            }
            if (parametrosCEM["IP"] != TextBoxIP.Text)
            {
                parametrosCEM["IP"] = TextBoxIP.Text;
            }
            if (parametrosCEM["Protocolo"] != protocolo)
            {
                parametrosCEM["Protocolo"] = protocolo;
            }
            return parametrosCEM;
        }
    }
}
