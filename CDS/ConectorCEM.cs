using Polly;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class ConectorCEM : IConector
    {
        private readonly byte separador = 0x7E;
        private readonly string nombreDelPipe = "CEM44POSPIPE";
        private string ipControlador = Configuration.LeerConfiguracion().IP;
        private string protocolo = ((InfoCEM)Configuration.LeerConfiguracion()).Protocolo;
        // Especifica la cultura que utiliza el punto como separador decimal
        private readonly CultureInfo culture = CultureInfo.InvariantCulture;
        public ConectorCEM() { }

        /*
         * Este método brinda toda la informacion de la configuracion del CEM-44,
         * con respecto a la estructura de la estacion para ser retornada.
         * Corresponde al "Comando de pedido de configuración de la estacón".
         */
        public List<Surtidor> GetSurtidores()
        {
            byte[] mensaje = protocolo.Equals("16") ? (new byte[] { 0x65 }) : (new byte[] { 0xB5 });
            int confirmacion = 0;
            int surtidores = 1;
            int tanques = 3;
            int productos = 4;
            Station tempStation = Station.InstanciaStation;

            byte[] respuesta = Log.Instance.GetLogLevel().Equals(Log.LogType.t_debug) ? LeerArchivo("ConfigEstacion") : EnviarComando(mensaje);
            try
            {
                if (respuesta == null || respuesta[confirmacion] != 0x0)
                {
                    throw new Exception("No se recibió mensaje de confirmación al solicitar informacion de los Surtidores.");
                }
                if (!File.Exists(Environment.CurrentDirectory + "/configEstacion.txt"))
                {
                    GuardarRespuesta(respuesta, "configEstacion.txt");
                }
                tempStation.NumeroDeSurtidores = respuesta[surtidores];
                tempStation.NumeroDeTanques = respuesta[tanques];
                tempStation.NumeroDeProductos = respuesta[productos];

                int posicion = productos + 1;
                List<Producto> tempProductos = new List<Producto>();
                for (int i = 0; i < tempStation.NumeroDeProductos; i++)
                {
                    Producto producto = new Producto
                    {
                        NumeroDeProducto = Convert.ToInt16(LeerCampoVariable(respuesta, ref posicion)),
                        PrecioUnitario = ConvertDouble(LeerCampoVariable(respuesta, ref posicion))
                    };
                    DescartarCampoVariable(respuesta, ref posicion);

                    switch (producto.NumeroDeProducto)
                    {
                        case 1:
                            producto.Descripcion = "SUPER";
                            break;
                        case 3:
                            producto.Descripcion = "ULTRA DIESEL";
                            break;
                        case 4:
                            producto.Descripcion = "INFINIA";
                            break;
                        case 6:
                            producto.Descripcion = "INFINIA DIESEL";
                            break;
                        case 8:
                            producto.Descripcion = "DIESEL 500";
                            break;
                        default:
                            producto.Descripcion = "N/Utilizado";
                            break;
                    }
                    tempProductos.Add(producto);
                }
                tempStation.Productos = tempProductos;

                List<Surtidor> tempSurtidores = new List<Surtidor>();
                for (int i = 0; i < tempStation.NumeroDeSurtidores; i++)
                {
                    Surtidor tempSurtidor = new Surtidor
                    {
                        NumeroDeSurtidor = i + 1,
                        NivelDeSurtidor = respuesta[posicion]
                    };
                    posicion++;
                    tempSurtidor.TipoDeSurtidor = respuesta[posicion] + 1;
                    posicion++;

                    for (int j = 0; j < tempSurtidor.TipoDeSurtidor; j++)
                    {
                        Manguera tempManguera = new Manguera
                        {
                            NumeroDeManguera = j + 1
                        };
                        foreach (Producto producto in tempStation.Productos)
                        {
                            if (producto.NumeroDeProducto == respuesta[posicion])
                            {
                                tempManguera.Producto = producto;
                                break;
                            }
                        }
                        tempSurtidor.Mangueras.Add(tempManguera);
                        posicion++;
                    }
                    tempSurtidores.Add(tempSurtidor);
                }
                tempStation.Surtidores = tempSurtidores;

                List<Tanque> tempTanques = new List<Tanque>();
                for (int i = 0; i < tempStation.NumeroDeTanques; i++)
                {
                    Tanque tanque = new Tanque
                    {
                        NumeroDeTanque = i + 1,
                        ProductoTanque = respuesta[posicion]
                    };
                    posicion++;
                    tempTanques.Add(tanque);
                }
                tempStation.Tanques = tempTanques;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al obtener información de los Surtidores. \nExcepción: {e.Message}");
            }
            return Station.InstanciaStation.Surtidores;
        }

        /*
         * Este método toma la estructura de los tanques, y pide la información
         * del estado de los tanques de combustible para ser retornada.
         * Corresponde al "Comando de pedido de stock de tanques"
         */
        public List<Tanque> GetTanques()
        {
            byte[] mensaje = protocolo.Equals("16") ? (new byte[] { 0x68 }) : (new byte[] { 0xB8 });
            int confirmacion = 0;
            byte[] respuesta = Log.Instance.GetLogLevel().Equals(Log.LogType.t_debug) ? LeerArchivo("infoTanques") : EnviarComando(mensaje);
            List<Tanque> tempTanques = Station.InstanciaStation.Tanques;

            try
            {
                if (respuesta == null || respuesta[confirmacion] != 0x0)
                {
                    throw new Exception("No se recibió mensaje de confirmación al solicitar info del surtidor");
                }

                int posicion = confirmacion + 1;

                for (int i = 0; i < tempTanques.Count; i++)
                {
                    foreach (Tanque tanque in tempTanques)
                    {
                        if (tanque.NumeroDeTanque == (i + 1))
                        {
                            tanque.NumeroDeTanque = i + 1;
                            tanque.VolumenProductoT = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                            tanque.VolumenAguaT = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                            tanque.VolumenVacioT = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al obtener informacion del tanque. \nExcepción: " + e.Message);
            }
            return Station.InstanciaStation.Tanques;
        }

        /*
         * Este método toma la estructura de los productos, previamente configurados,
         * para ser retornada.
         * Corresponde al "Comando de pedido de stock de tanques"
         */
        public List<Producto> GetProductos()
        {
            return Station.InstanciaStation.Productos;
        }

        /*
         * Este método toma la estructura de un despacho y por cada iteracion, completa
         * el despacho de la UltimaVenta y la VentaAnterior, si es que existen.
         */
        public List<Despacho> GetDespacho()
        {
            byte[] mensaje = protocolo.Equals("16") ? (new byte[] { 0x70 }) : (new byte[] { 0xC0 });
            int confirmacion = 0;
            int status = 1;
            int nro_venta = 2;
            int codigo_producto = 3;
            List<Despacho> despachos = new List<Despacho>();

            for (int numeroDeSurtidor = 1; numeroDeSurtidor <= Station.InstanciaStation.NumeroDeSurtidores; numeroDeSurtidor++)
            {
                byte[] respuesta = Log.Instance.GetLogLevel().Equals(Log.LogType.t_debug) ?
                    LeerArchivo("despacho-" + numeroDeSurtidor) :
                    EnviarComando(new byte[] { (byte)(mensaje[0] + Convert.ToByte(numeroDeSurtidor)) });
                try
                {
                    if (respuesta == null || respuesta[confirmacion] != 0x0)
                    {
                        throw new Exception("No se recibió mensaje de confirmación al solicitar info del surtidor.");
                    }
                    if (!File.Exists(Environment.CurrentDirectory + $"/despacho-{numeroDeSurtidor}.txt"))
                    {
                        GuardarRespuesta(respuesta, $"despacho-{numeroDeSurtidor}.txt");
                    }

                    int posicion = codigo_producto + 1;

                    // Proceso ultima venta
                    byte statusUltimaVenta = respuesta[status];

                    // Verificamos si es un SurtidorDespachando o SurtidorDetenido
                    int numeroUltimaVenta = respuesta[nro_venta];
                    if (statusUltimaVenta != 0x03 && statusUltimaVenta != 0x0A && numeroUltimaVenta != 0)
                    {
                        Despacho ultimaVenta = new Despacho
                        {
                            NumeroUltimaVenta = numeroUltimaVenta,
                            Producto = respuesta[codigo_producto],
                            Monto = ConvertDouble(LeerCampoVariable(respuesta, ref posicion)),
                            Volumen = ConvertDouble(LeerCampoVariable(respuesta, ref posicion)),
                            PPU = ConvertDouble(LeerCampoVariable(respuesta, ref posicion)),
                            Facturado = Convert.ToBoolean(respuesta[posicion]),
                            Surtidor = numeroDeSurtidor
                        };
                        posicion++;
                        ultimaVenta.ID = Convert.ToInt32(LeerCampoVariable(respuesta, ref posicion));
                        ultimaVenta.Descripcion = GetProduct(ultimaVenta.Producto).Descripcion;

                        if (ultimaVenta.Facturado || ultimaVenta.PPU < GetProduct(ultimaVenta.Producto).PrecioUnitario)
                        {
                            ultimaVenta.YPFRuta = true;
                        }

                        despachos.Add(ultimaVenta);
                    }
                    else
                    {
                        posicion = status + 1;
                    }

                    //Proceso venta anterior
                    byte statusVentaAnterior = respuesta[posicion];
                    posicion++;
                    int numeroVentaAnterior = respuesta[posicion];
                    if (numeroVentaAnterior != 0 && statusUltimaVenta != 0x03 && statusUltimaVenta != 0x0A)
                    {
                        posicion++;
                        Despacho ventaAnterior = new Despacho
                        {
                            NumeroUltimaVenta = numeroVentaAnterior,
                            Producto = respuesta[posicion],
                            Surtidor = numeroDeSurtidor
                        };
                        posicion++;
                        ventaAnterior.Monto = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                        ventaAnterior.Volumen = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                        ventaAnterior.PPU = ConvertDouble(LeerCampoVariable(respuesta, ref posicion));
                        ventaAnterior.Facturado = Convert.ToBoolean(respuesta[posicion]);
                        posicion++;
                        ventaAnterior.ID = Convert.ToInt32(LeerCampoVariable(respuesta, ref posicion));
                        ventaAnterior.Descripcion = GetProduct(ventaAnterior.Producto).Descripcion;

                        if (ventaAnterior.Facturado || ventaAnterior.PPU < GetProduct(ventaAnterior.Producto).PrecioUnitario)
                        {
                            ventaAnterior.YPFRuta = true;
                        }

                        despachos.Add(ventaAnterior);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("\nError al obtener informacion del despacho.\n\tExcepcion: " + e.Message);
                }
            }
            return despachos;
        }

        /*
         * Este método es el que se conecta con el CEM-44 para enviar el comando
         * y obtener la respuesta a dicho comando.
         */
        private byte[] EnviarComando(byte[] comando)
        {
            try
            {
                byte[] buffer = null;
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(ipControlador, nombreDelPipe))
                {
                    int retries = 1;

                    PolicyResult policyResult = Policy.Handle<Exception>().WaitAndRetry
                        (
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (exception, TimeSpan, context) =>
                        {
                            pipeClient.Dispose();
                            //_ = Log.Instance.WriteLog($"\n\t  Excepción: {exception.Message.Trim()} Intento: {retries}", Log.LogType.t_error);
                            retries++;
                        }).ExecuteAndCapture(() =>
                        {
                            pipeClient.Connect(5000);
                            pipeClient.Write(comando, 0, comando.Length);
                            buffer = new byte[pipeClient.OutBufferSize];
                            _ = pipeClient.Read(buffer, 0, buffer.Length);
                        });
                    //Controlador.CheckConexion((int)policyResult.Outcome);
                    if (policyResult.Outcome != 0)
                    {
                        //_ = Log.Instance.WriteLog($"  Fin de intentos...\n", Log.LogType.t_error);
                        pipeClient.Close();
                        ReloadData();
                    }
                }
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al enviar el comando. \nExcepción: {e.Message}");
            }
        }

        private Producto GetProduct(int numbProducto)
        {
            Producto product = null;
            foreach (Producto producto in Station.InstanciaStation.Productos)
            {
                if (producto.NumeroDeProducto == numbProducto)
                {
                    product = producto;
                    break;
                }
            }
            return product;
        }

        /*
         * En caso de completar los intentos de reconexión y finalmente fallar,
         * verifica si existe un cambio en los parametros claves para la conexión.
         * De esta manera, para el proximo ciclo de intentos estarán actualizados.
         */
        private void ReloadData()
        {
            ipControlador = ((InfoCEM)Configuration.LeerConfiguracion()).IP;
            protocolo = ((InfoCEM)Configuration.LeerConfiguracion()).Protocolo;
        }

        /*
         * Metodo para leer los campos variables, por ejemplo precios o cantidades.
         * El metodo para frenar la iteracion es un valor conocido, proporcionado por el fabricante,
         * denominado como "separador".
         */
        private string LeerCampoVariable(byte[] data, ref int pos)
        {
            string ret = "";
            ret += Encoding.ASCII.GetString(new byte[] { data[pos] });
            int i = pos + 1;
            while (data[i] != separador)
            {
                ret += Encoding.ASCII.GetString(new byte[] { data[i] });
                i++;
            }
            i++;
            pos = i;
            return ret;
        }

        /*
         * Metodo para saltearse los valores que no son utilizados en la respuesta del CEM.
         * Al finalizar el proceso del metodo, el valor de la posicion queda seteada para
         * el siguiente dato a procesar.
         */
        private void DescartarCampoVariable(byte[] data, ref int pos)
        {
            while (data[pos] != separador)
            {
                pos++;
            }
            pos++;
        }

        /*
         * Metodo que se utiliza para darle formato a los valores decimales
         * pero no tiene en cuenta la configuracion del sistema operativo.
         * Proporciona ventaja en el sentido que para todas las maquinas va a 
         * funcionar de la misma manera.
         */
        private double ConvertDouble(string value)
        {
            return double.TryParse(value, NumberStyles.Any, culture, out double result) ? result : result;
        }

        /*
         * Se utiliza para testear las respuestas reales del Cem-44
         * se lee un .txt que contiene las respuestas y las guarda en un byte,
         * para simular la respuesta.
         */
        private byte[] LeerArchivo(string nombreArchivo)
        {
            byte[] respuesta = null;
            // Obtener la ruta del directorio donde se ejecuta el programa
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;

            // Combinar la ruta del directorio con el nombre del archivo
            string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo + ".txt");

            // Verificar si el archivo existe
            if (File.Exists(rutaArchivo))
            {
                // Leer todas las líneas del archivo
                string[] lines = File.ReadAllLines(rutaArchivo);

                // Arreglo para almacenar todos los bytes
                byte[] byteArray = new byte[lines.Length * 4]; // Se asume que cada fila tiene 4 valores numéricos (4 bytes cada uno)

                // Índice para rastrear la posición en el arreglo de bytes
                int index = 0;

                // Procesar cada línea del archivo
                foreach (string line in lines)
                {
                    // Dividir la línea en valores numéricos individuales
                    string[] numericValues = line.Split(',');

                    // Convertir cada valor numérico en un byte y agregarlos al arreglo de bytes
                    foreach (string value in numericValues)
                    {
                        byteArray[index++] = Convert.ToByte(value.Trim());
                    }
                }
                respuesta = byteArray;
            }
            return respuesta;
        }

        /*
         * Con este metodo se pueden tomar muestras de respuesta y guardarlas
         * como un archivo.txt, para los diferentes casos como Despachos, Surtidores,
         * Tanques y Cierres de Turno.
         */
        private void GuardarRespuesta(byte[] respuesta, string nombreArchivo)
        {
            // Obtener la ruta del archivo en la misma carpeta donde se ejecuta el programa
            //string nombreArchivo = "respuesta.txt";
            string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nombreArchivo);

            if (!File.Exists(nombreArchivo))
            {
                // Escribir en el archivo
                using (StreamWriter sw = File.AppendText(rutaCompleta))
                {
                    int cont = 0;
                    int iteraciones = 0;
                    int iterAnt = 0;
                    while (iteraciones < respuesta.Length)
                    {
                        sw.WriteLine(respuesta[iteraciones]);
                        if (iterAnt > 0)
                        {
                            if (respuesta[iteraciones] == 0 && respuesta[iterAnt] == 0 && cont < 6)
                            {
                                cont++;
                            }
                            else if (respuesta[iteraciones] == 0 && cont >= 6)
                            {
                                break;
                            }
                        }
                        iteraciones++;
                        iterAnt = iteraciones - 1;
                    }
                }
            }
        }
    }
}
