using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class Structure
    {
        #region ESTACION
        public class Station
        {
            public Station()
            {
                Surtidores = new List<Surtidor>();
                Tanques = new List<Tanque>();
                Productos = new List<Producto>();
            }
            public static Station InstanciaStation { get; } = new Station();
            public int NumeroDeSurtidores { get; set; }
            public int NumeroDeTanques { get; set; }
            public int NumeroDeProductos { get; set; }
            public List<Surtidor> Surtidores { get; set; }
            public List<Tanque> Tanques { get; set; }
            public List<Producto> Productos { get; set; }
        }
        public class Surtidor
        {
            public Surtidor()
            {
                NivelDeSurtidor = 0;
                TipoDeSurtidor = 0;
                Mangueras = null;
                NumeroDeSurtidor = 0;
            }
            public int NivelDeSurtidor { get; set; }  // indica al nivel de precio al que trabaja
            public int TipoDeSurtidor { get; set; }   // indica la cantidad de mangueras que tiene ese surtidor
            public List<Manguera> Mangueras { get; set; }
            public int NumeroDeSurtidor { get; set; }
        }
        public class Tanque
        {
            public Tanque()
            {
                NumeroDeTanque = 0;
                ProductoTanque = 0;
                VolumenProductoT = 0;
                VolumenAguaT = 0;
                VolumenVacioT = 0;
                Producto = null;
            }
            public int NumeroDeTanque { get; set; }
            public int ProductoTanque { get; set; }
            public double VolumenProductoT { get; set; }
            public double VolumenAguaT { get; set; }
            public double VolumenVacioT { get; set; }
            public Producto Producto { get; set; }
        }
        public class Manguera
        {
            public Manguera()
            {
                NumeroDeManguera = 0;
                Producto = null;
            }
            public int NumeroDeManguera { get; set; }
            public Producto Producto { get; set; }
        }
        public class Producto
        {
            public Producto()
            {
                NumeroDeProducto = 0;
                PrecioUnitario = 0;
                Descripcion = "";
            }
            public int NumeroDeProducto { get; set; }
            public double PrecioUnitario { get; set; }
            public string Descripcion { get; set; }
        }
        #endregion
    }

}
