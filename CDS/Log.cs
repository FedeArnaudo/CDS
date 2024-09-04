using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class Log
    {
        public enum LogType
        {
            t_normal = 0,
            t_debug,
            t_error
        }

        /*  Esta variable almacena el tipo de log que esta seteando como maximo.
            Por ejemplo, si se setea como t_warning, solo los mensajes del tipo t_warning y t_error
            se van a mostar. Si se pone en t_debug, todos los mensajes se muestran.
        */
        private LogType logLevel = LogType.t_debug;

        private Log() { }

        // Singleton implementation
        public static Log Instance { get; } = new Log();

        public void SetLogLevel(LogType level)
        {
            logLevel = level;
        }
        public LogType GetLogLevel()
        {
            return logLevel;
        }

        public void WriteLog(string message, LogType type)
        {
            try
            {
                // Crea la carpeta si no existe, en la carpeta base del programa.
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Log";
                string logFile = "log" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

                if (!Directory.Exists(path))
                {
                    _ = Directory.CreateDirectory(path);
                }

                // Borra el log del dia anterior
                string deleteFile = "log" + DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(Environment.CurrentDirectory + "/Log/" + deleteFile))
                {
                    File.Delete(Environment.CurrentDirectory + "/Log/" + deleteFile);
                }

                // Log message with correspondant log level
                switch (type)
                {
                    case LogType.t_normal:
                        if (logLevel <= type)
                        {
                            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, logFile), true))
                            {
                                outputFile.WriteLine(DateTime.Now.ToString("hh:mm:ss") + "  INFO:    " + message);
                            }
                        }
                        break;
                    case LogType.t_debug:
                        if (logLevel <= type)
                        {
                            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, logFile), true))
                            {
                                outputFile.WriteLine(DateTime.Now.ToString("hh:mm:ss") + "  DEBUG:   " + message);
                            }
                        }
                        break;
                    case LogType.t_error:
                        if (logLevel <= type)
                        {
                            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, logFile), true))
                            {
                                outputFile.WriteLine(DateTime.Now.ToString("hh:mm:ss") + "  ERROR:   " + message);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error al escribir el reporte en el log. Excepcion: {e.Message}");
            }
        }
    }
}
