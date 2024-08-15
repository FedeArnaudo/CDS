﻿using System;
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
                switch (reader.ReadLine().Trim())
                {
                    case "CEM-44":
                        infoConfig = new InfoCEM()
                        {
                            TipoDeControlador = reader.ReadLine().Trim(),
                            RutaProyNuevo = reader.ReadLine().Trim(),
                            IP = reader.ReadLine().Trim(),
                            Protocolo = reader.ReadLine().Trim()
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
                    outputFile.WriteLine(infoConfig.TipoDeControlador.Trim());
                    outputFile.WriteLine(infoConfig.RutaProyNuevo.Trim());
                    outputFile.WriteLine(infoConfig.IP.Trim());
                    switch (infoConfig.TipoDeControlador)
                    {
                        case "CEM-44":
                            InfoCEM infoCEM = (InfoCEM)infoConfig;
                            outputFile.WriteLine(infoCEM.Protocolo.Trim());
                            break;
                        case "FUSION":
                            break;
                        default:
                            break;
                    }
                    outputFile.WriteLine(infoConfig.InfoLog);
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
        protected string rutaProyNuevo = "";
        private string ip = "";
        protected string infoLog = "";
        public Info() { }
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
        public string InfoLog
        {
            get => infoLog;
            set
            {
                if(infoLog != value)
                {
                    infoLog = value;
                }
            }
        }
    }
    #endregion
    #region INFO_CEM
    public class InfoCEM : Info
    {
        private string protocolo = "";
        public InfoCEM() { }
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
        public InfoFusion() { }
    }
    #endregion
}
