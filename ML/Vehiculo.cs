using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Vehiculo
    {

        public int NumeroReclamo { get; set; }
        public string FechaReclamo { get; set; }
        public int HoraPercanse { get; set; }
        public string TipoPercanse { get; set; }
        public int NumeroPoliza { get; set; }
        public string NombreConductor { get; set; }
        public int ContactoConductor { get; set; }
        public string DetallesVehiculo { get; set; }
        public string DañosPrejuicios { get; set; }
        public decimal EstimacionReparacion { get; set; }

        public List<object> Vehiculos { get; set; }


    }
}
