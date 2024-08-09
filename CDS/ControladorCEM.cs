using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CDS.Structure;

namespace CDS
{
    internal class ControladorCEM : Controlador
    {
        public ControladorCEM()
        {
            Conector = new ConectorCEM();
        }
        public override void GrabarCierreAnterior()
        {
            throw new NotImplementedException();
        }

        public override void GrabarCierreDeTurno()
        {
            throw new NotImplementedException();
        }

        public override void GrabarConfigEstacion()
        {
            throw new NotImplementedException();
        }

        public override void GrabarDespachos()
        {
            throw new NotImplementedException();
        }

        public override void GrabarProductos()
        {
            //List<Producto> productos = Conector.GetProductos();

            throw new NotImplementedException();
        }

        public override void GrabarTanques()
        {
            throw new NotImplementedException();
        }

        public override void GrabarTurnoEnCurso()
        {
            throw new NotImplementedException();
        }
    }
}
