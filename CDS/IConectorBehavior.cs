using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal interface IConectorBehavior
    {
        List<Structure.Surtidor> GetSurtidores();
        List<Structure.Tanque> GetTanques();
        List<Structure.Producto> GetProductos();
    }
}
