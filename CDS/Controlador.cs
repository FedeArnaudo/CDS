using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CDS
{
    internal abstract class Controlador
    {
        // Instancia de Singleton
        private static Controlador instancia = null;
        // Hilo para manejar el proceso principal de consulta al controlador en paralelo al resto de la ejecución
        private static readonly Thread procesoPrincipal = null;
        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        protected IConector conector;
        public static bool endWork = false;
        public Controlador() { }
        public IConector Conector
        {
            get => conector;
            set
            {
                if (!conector.Equals(value))
                {
                    conector = value;
                }
            }
        }
        /// <summary>
        /// Este método estático es el encargado de configurar la estructura de la estacion,
        /// para obtener los productos, los tanques, las mangueras y surtidores, etc.
        /// Y se guarda la informacion en la tabla de la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarSurtidores();

        /// <summary>
        /// Este método estático es el encargado de procesar la informacion de los surtidores
        /// y guardarla en la tabla de la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarDespachos();

        /// <summary>
        /// Este método estático es el encargado de procesar la informacion de los tanques
        /// y guardarla en la tabla de la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarTanques();

        /// <summary>
        /// Este método estático es el encargado de grabar los productos que trae el controlador
        /// y guardarla en la tabla de la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarProductos();

        /// <summary>
        /// Este método estático es el encargado de procesar la informacion del ultimo turno,
        /// sin realizar el corte y guardarla en la tabla de la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarTurnoEnCurso();

        /// <summary>
        /// Este método estático es el encargado de procesar la informacion del corte del ultimo turno
        /// y guardarla en la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarCierreActual();

        /// <summary>
        /// Este método estático es el encargado de procesar la informacion del cierre del turno anterior
        /// y guardarla en la base de datos correspondiente.
        /// </summary>
        public abstract void GrabarCierreAnterior();

        /// <summary>
        /// Este método estático es el encargado de crear la instancia del controlador
        /// correspondiente y ejecutar el hilo del proceso automático
        /// </summary>
        /// <param name="config"> La configuración extraída del archivo de configuración </param>
        /// <returns> true si se pudo inicializar correctamente </returns>
        public static bool Init(string controlador)
        {
            if (instancia == null)
            {
                ConectorSQLite.CrearBBDD();
                switch (controlador)
                {
                    case "CEM-44":
                        instancia = new ControladorCEM();
                        break;
                    case "FUSION":
                        break;
                    default:
                        return false;
                }

                if (procesoPrincipal == null || !procesoPrincipal.IsAlive)
                {
                    _ = Task.Run(() => Run(cancellationTokenSource.Token));
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private static void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    instancia.GrabarSurtidores();
                    instancia.GrabarProductos();
                    instancia.GrabarTanques();
                    while (true)
                    {
                        instancia.GrabarDespachos();

                        // Espera para procesar nuevamente
                        Thread.Sleep(500);

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                        else if (ControladorCEM.PedirCierreAnterior)
                        {
                            instancia.GrabarCierreAnterior();
                            ControladorCEM.PedirCierreAnterior = false;
                        }
                        else if (ControladorCEM.PedirTurnoActual)
                        {
                            instancia.GrabarTurnoEnCurso();
                            ControladorCEM.PedirTurnoActual = false;
                        }
                        else if (ControladorCEM.PedirStockDeTanques)
                        {
                            instancia.GrabarTanques();
                            ControladorCEM.PedirStockDeTanques = false;
                        }

                        // Espera para procesar nuevamente
                        Thread.Sleep(500);
                    }
                    instancia.GrabarCierreActual();
                    // Espera para procesar nuevamente
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error en el loop del controlador.\nExcepción: {e.Message}\n");
                    //_ = Log.Instance.WriteLog($"Error en el loop del controlador.\nExcepción: {e.Message}\n", Log.LogType.t_error);
                }
            }
        }
    }
}
