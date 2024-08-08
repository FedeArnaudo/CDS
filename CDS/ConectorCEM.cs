using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS
{
    internal class ConectorCEM : IConectorBehavior
    {
        private readonly byte separador = 0x7E;
        private readonly string nombreDelPipe = "CEM44POSPIPE";
        private readonly string ipControlador = ((InfoCEM)Configuration.LeerConfiguracion()).IP;
        private readonly string protocolo = ((InfoCEM)Configuration.LeerConfiguracion()).Protocolo;
        // Especifica la cultura que utiliza el punto como separador decimal
        private readonly CultureInfo cultureculture = CultureInfo.InvariantCulture;
        public ConectorCEM()
        {

        }

        public List<Structure.Surtidor> GetSurtidores()
        {
            byte[] mensaje = protocolo.Equals("16") ? (new byte[] { 0x65 }) : (new byte[] { 0xB5 });
            int confirmacion = 0;
            int surtidores = 1;
            int tanques = 3;
            int productos = 4;

            throw new NotImplementedException();
        }
    }
}
