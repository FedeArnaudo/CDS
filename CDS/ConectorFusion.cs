using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class ConectorFusion : IConector
    {
        public Despacho GetDespacho()
        {
            throw new NotImplementedException();
        }

        public List<Producto> GetProductos()
        {
            throw new NotImplementedException();
        }

        public List<Surtidor> GetSurtidores()
        {
            throw new NotImplementedException();
        }

        public List<Tanque> GetTanques()
        {
            throw new NotImplementedException();
        }

        List<Despacho> IConector.GetDespacho()
        {
            throw new NotImplementedException();
        }
    }
}
