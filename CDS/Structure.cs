using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
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
    #region DESPACHOS
    public class Despacho
    {
        public Despacho() { }

        public enum ESTADO_SURTIDOR
        {
            DISPONIBLE,
            EN_SOLICITUD,
            DESPACHANDO,
            AUTORIZADO,
            VENTA_FINALIZADA_IMPAGA,
            DEFECTUOSO,
            ANULADO,
            DETENIDO
        }
        public ESTADO_SURTIDOR StatusUltimaVenta { get; set; }
        public int NroUltimaVenta { get; set; }
        public int ProductoUltimaVenta { get; set; }
        public double MontoUltimaVenta { get; set; }
        public double VolumenUltimaVenta { get; set; }
        public double PpuUltimaVenta { get; set; }
        public bool UltimaVentaFacturada { get; set; }
        public int IdUltimaVenta { get; set; }

        public ESTADO_SURTIDOR StatusVentaAnterior { get; set; }
        public int NroVentaAnterior { get; set; }
        public int ProductoVentaAnterior { get; set; }
        public double MontoVentaAnterior { get; set; }
        public double VolumenVentaAnterios { get; set; }
        public double PpuVentaAnterior { get; set; }
        public bool VentaAnteriorFacturada { get; set; }
        public int IdVentaAnterior { get; set; }
    }
    public class InfoDespacho
    {
        public InfoDespacho() { }
        public int ID { get; set; }
        public int Surtidor { get; set; }
        public string Manguera { get; set; }
        public int Producto { get; set; }
        public double Monto { get; set; }
        public double Volumen { get; set; }
        public double PPU { get; set; }
        public bool Facturado { get; set; }
        public bool YPFRuta { get; set; }
        public string Desc { get; set; }
        public string Fecha { get; set; }
    }
    #endregion
}
