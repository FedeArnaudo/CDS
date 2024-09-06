using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CDS.Views
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

        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e)
        {
            Info infoConfig;
            Dictionary<string, string> parametros = null;
            if (ComboBoxTipo.Text != "")
            {

                if (CkeckParametros(parametros))
                {
                    // ####### ELIMINA ##################################################
                    string info = $"Configuracion ingresada:\n";
                    foreach (KeyValuePair<string, string> parametro in parametros)
                    {
                        info += $"->    {parametro.Key}: {parametro.Value}\n";
                    }
                    _ = MessageBox.Show(info);
                    // ##################################################################

                    switch (ComboBoxTipo.Text)
                    {
                        case "CEM-44":
                            infoConfig = new InfoCEM()
                            {
                                TipoDeControlador = ComboBoxTipo.Text,
                                RutaProyNuevo = parametros["Ruta"],
                                IP = parametros["IP"],
                                Protocolo = parametros["Protocolo"],
                                Modo = ComboBoxMode.Text
                            };
                            break;
                        case "FUSION":
                            infoConfig = new InfoFusion()
                            {
                                TipoDeControlador = ComboBoxTipo.Text,
                                RutaProyNuevo = parametros["Ruta"],
                                Modo = ComboBoxMode.Text
                            };
                            break;
                        default:
                            infoConfig = new Info();
                            break;
                    }
                    if (Configuration.GuardarConfiguracion(infoConfig))
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
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Combinación de teclas Ctrl + E
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.E)
                {
                    ComboBoxMode.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
