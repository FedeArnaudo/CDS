using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class Configuration
    {

        private static readonly string configFile = Environment.CurrentDirectory + "/config.ini";
        public Configuration() { }
        public static Info LeerConfiguracion()
        {
            Info infoConfig = null;
            try
            {
                StreamReader reader;
                reader = new StreamReader(configFile);
                switch (reader.ReadLine().Trim())                                      //1ro    Controlador
                {
                    case "CEM-44":
                        infoConfig = new InfoCEM()
                        {
                            RutaProyNuevo = reader.ReadLine().Trim(),                  //2do    Ruta
                            IP = reader.ReadLine().Trim(),                             //3ro    IP
                            TimeSleep = Convert.ToInt32(reader.ReadLine().Trim()),     //4to    Tiempo entre consultas
                            Modo = reader.ReadLine().Trim(),                           //5to    Modo
                            Protocolo = reader.ReadLine().Trim()                       //6to    Protocolo
                        };
                        break;
                    case "FUSION":
                        infoConfig = new InfoFusion();
                        break;
                    default:
                        infoConfig = new Info();
                        break;
                }
                reader.Close();
                return infoConfig;
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog("Error al leer archivo de configuración. Formato incorrecto. Excepción: " + e.Message, Log.LogType.t_error);
                //Console.WriteLine($"Error al leer archivo de configuración. Formato incorrecto. Excepción: {e.Message}");
                return infoConfig;
            }
        }
        public static bool GuardarConfiguracion(Info infoConfig)
        {
            try
            {
                //Crea el archivo config.ini
                using (StreamWriter outputFile = new StreamWriter(configFile, false))
                {
                    outputFile.WriteLine(infoConfig.TipoDeControlador.Trim());              //1ro   Controlador
                    outputFile.WriteLine(infoConfig.RutaProyNuevo.Trim());                  //2do   Ruta
                    outputFile.WriteLine(infoConfig.IP.Trim());                             //3ro   IP
                    outputFile.WriteLine(infoConfig.TimeSleep);                             //4to   Tiempo entre consultas
                    outputFile.WriteLine(infoConfig.Modo);                                  //5to   Modo
                    switch (infoConfig.TipoDeControlador)
                    {
                        case "CEM-44":
                            InfoCEM infoCEM = (InfoCEM)infoConfig;
                            outputFile.WriteLine(infoCEM.Protocolo.Trim());                 //6to   Protocolo
                            break;
                        case "FUSION":
                            break;
                        default:
                            break;
                    }
                    
                }
            }
            catch (Exception e)
            {
                Log.Instance.WriteLog($"Error al guardar la configuración. Excepción: {e.Message}", Log.LogType.t_error);
                //Console.WriteLine($"Error al guardar la configuración. Excepción: {e.Message}");
                return false;
            }
            return true;
        }
        public static bool ExisteConfiguracion()
        {
            return File.Exists(configFile);
        }
    }
    #region INFO
    public class Info
    {
        protected string tipoDeControlador = "";
        private string rutaProyNuevo = "";
        private string ip = "";
        private int timeSleep = 0;
        protected MODO modo = MODO.NORMAL;
        public Info() { }
        public enum MODO
        {
            NORMAL,
            DEBUG
        }
        public string TipoDeControlador
        {
            get => tipoDeControlador;
            set
            {
                if (tipoDeControlador != value)
                {
                    tipoDeControlador = value;
                }
            }
        }
        public string RutaProyNuevo
        {
            get => rutaProyNuevo;
            set
            {
                if (rutaProyNuevo != value)
                {
                    rutaProyNuevo = value;
                }
            }
        }
        public string IP
        {
            get => ip;
            set
            {
                if (ip != value)
                {
                    ip = value;
                }
            }
        }
        public int TimeSleep
        {
            get => timeSleep;
            set
            {
                if (timeSleep != value)
                {
                    timeSleep = value;
                }
            }
        }
        public string Modo
        {
            get => modo.ToString();
            set
            {
                modo = MODO.NORMAL;
                if (value.Contains(MODO.DEBUG.ToString()))
                {
                    modo = MODO.DEBUG;
                }
            }
        }
    }
    #endregion
    #region INFO_CEM
    public class InfoCEM : Info
    {
        private string protocolo = "";
        public InfoCEM()
        {
            TipoDeControlador = "CEM-44";
        }
        public string Protocolo
        {
            get => protocolo;
            set
            {
                if (protocolo != value)
                {
                    protocolo = value;
                }
            }
        }
    }
    #endregion
    #region INFO_FUSION
    public class InfoFusion : Info
    {
        public InfoFusion()
        {
            TipoDeControlador = "FUSION";
        }
    }
    #endregion
}
