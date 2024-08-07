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

namespace CDS
{
    /// <summary>
    /// Interaction logic for SettingsFusion.xaml
    /// </summary>
    public partial class SettingsFusion : Page
    {
        private readonly Dictionary<string, string> parametrosFusion = new Dictionary<string, string>();
        public SettingsFusion()
        {
            InitializeComponent();
            parametrosFusion.Add("Ruta", "");
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
        public Dictionary<string, string> GetConfiguration()
        {
            if (parametrosFusion["Ruta"] != TextBoxPryNuevo.Text)
            {
                parametrosFusion["Ruta"] = TextBoxPryNuevo.Text;
            }
            return parametrosFusion;
        }
    }
}
