using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class Log
    {
        public enum LogType
        {
            t_debug = 0,
            t_normal,
            t_warning,
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
    }
}
