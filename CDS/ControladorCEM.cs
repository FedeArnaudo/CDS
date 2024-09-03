using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class ControladorCEM : Controlador
    {
        public static bool PedirCierreAnterior { get; set; }
        public static bool PedirTurnoActual { get; set; }
        public static bool PedirStockDeTanques { get; set; }
        public ControladorCEM()
        {
            Conector = new ConectorCEM();
            PedirCierreAnterior = false;
            PedirTurnoActual = false;
            PedirStockDeTanques = false;
        }

        public override void GrabarSurtidores()
        {
            List<Surtidor> surtidores = Conector.GetSurtidores();
            try
            {
                string campos = "IdSurtidor,Manguera,Producto,Precio,DescProd";
                foreach (Surtidor surtidor in surtidores)
                {
                    List<Manguera> mangueras = surtidor.Mangueras;
                    foreach (Manguera manguera in mangueras)
                    {
                        string rows = string.Format("{0},{1},{2},{3},'{4}'",
                            surtidor.NumeroDeSurtidor,
                            manguera.NumeroDeManguera,
                            manguera.Producto.NumeroDeProducto,
                            manguera.Producto.PrecioUnitario.ToString(),
                            manguera.Producto.Descripcion);

                        DataTable tabla = ConectorSQLite.Dt_query("SELECT * FROM Surtidores WHERE IdSurtidor = " + surtidor.NumeroDeSurtidor + " AND Manguera = '" + manguera.NumeroDeManguera + "'");

                        _ = tabla.Rows.Count == 0
                            ? ConectorSQLite.Query(string.Format("INSERT INTO Surtidores ({0}) VALUES ({1})", campos, rows))
                            : ConectorSQLite.Query(string.Format("UPDATE Surtidores SET Producto = ('{0}'), Precio = ('{1}'), DescProd = ('{2}') " +
                                                                 "WHERE IdSurtidor = ({3}) AND Manguera = ('{4}')",
                                manguera.Producto.NumeroDeProducto,
                                manguera.Producto.PrecioUnitario.ToString(),
                                manguera.Producto.Descripcion,
                                surtidor.NumeroDeSurtidor,
                                manguera.NumeroDeManguera));
                        Log.Instance.WriteLog(string.Format("SURT: ({0}) Cem44: ({1}) Desc: ({2})", manguera.NumeroDeManguera, surtidor.NumeroDeSurtidor + manguera.NumeroDeManguera, manguera.Producto.Descripcion), Log.LogType.t_normal);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error en el metodo GrabarSurtidores. Excepcion: {e.Message}");
            }
        }

        public override void GrabarDespachos()
        {
            throw new NotImplementedException();
        }

        public override void GrabarTanques()
        {
            throw new NotImplementedException();
        }

        public override void GrabarProductos()
        {
            throw new NotImplementedException();
        }

        public override void GrabarTurnoEnCurso()
        {
            throw new NotImplementedException();
        }

        public override void GrabarCierreActual()
        {
            throw new NotImplementedException();
        }

        public override void GrabarCierreAnterior()
        {
            throw new NotImplementedException();
        }
    }
}
