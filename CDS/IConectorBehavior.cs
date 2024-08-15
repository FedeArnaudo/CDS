using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal interface IConectorBehavior
    {
        List<Surtidor> GetSurtidores();
        List<Tanque> GetTanques();
        List<Producto> GetProductos();
    }
}
