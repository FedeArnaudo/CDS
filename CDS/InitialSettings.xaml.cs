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
using System.Windows.Shapes;

namespace CDS
{
    /// <summary>
    /// Interaction logic for InitialSettings.xaml
    /// </summary>
    public partial class InitialSettings : Window
    {
        private Page pageController = new Page();
        public InitialSettings()
        {
            InitializeComponent();
        }

        private void ComboBoxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrameController != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)ComboBoxTipo.SelectedItem;

                if (selectedItem != null)
                {
                    switch (selectedItem.Content.ToString())
                    {
                        case "CEM-44":
                            pageController = new SettingsCEM();
                            _ = FrameController.Navigate(pageController);
                            break;
                        case "FUSION":
                            pageController = new SettingsFusion();
                            _ = FrameController.Navigate(pageController);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            Info infoConfig;
            Dictionary<string, string> parametros = null;
            if (ComboBoxTipo.Text != "")
            {
                if (FrameController.Content is SettingsCEM CEM)
                {
                    parametros = CEM.GetConfiguration();
                }
                else if (FrameController.Content is SettingsFusion Fusion)
                {
                    parametros = Fusion.GetConfiguration();
                }

                if (CkeckParametros(parametros))
                {
                    string info = $"Configuracion ingresada:\n";
                    foreach (KeyValuePair<string, string> parametro in parametros)
                    {
                        info += $"->    {parametro.Key}: {parametro.Value}\n";
                    }
                    _ = MessageBox.Show(info);

                    switch (ComboBoxTipo.Text)
                    {
                        case "CEM-44":
                            infoConfig = new InfoCEM()
                            {
                                TipoDeControlador = ComboBoxTipo.Text,
                                RutaProyNuevo = parametros["Ruta"],
                                IP = parametros["IP"],
                                Protocolo = parametros["Protocolo"]
                            };
                            break;
                        case "FUSION":
                            infoConfig = new InfoFusion()
                            {
                                TipoDeControlador = ComboBoxTipo.Text,
                                RutaProyNuevo = parametros["Ruta"]
                            };
                            break;
                        default:
                            infoConfig = new Info();
                            break;
                    }
                    if (Configuracion.GuardarConfiguracion(infoConfig))
                    {
                        //_ = Log.Instance.WriteLog($"La configuración se guardó correctamente.", Log.LogType.t_info);
                        Console.WriteLine($"La configuración se guardó correctamente.");
                        _ = MessageBox.Show($"La configuración se guardó correctamente.");
                        Close();
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Debe ingresar el tipo de controlador.");
            }
        }
        private bool CkeckParametros(Dictionary<string, string> parametros)
        {
            foreach (KeyValuePair<string, string> parametro in parametros)
            {
                if (parametro.Value == "")
                {
                    _ = MessageBox.Show($"Falta completar el parametro: {parametro.Key}");
                    return false;
                }
            }
            return true;
        }
    }
}
