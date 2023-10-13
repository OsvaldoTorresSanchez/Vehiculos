using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Vehiculo
    {
        public static ML.Result GetAllEF()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.VehiculosEntities context = new DL.VehiculosEntities())
                {
                    var resultQuery = context.VehiculoGetAll().ToList();


                    if (resultQuery != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in resultQuery)
                        {
                            ML.Vehiculo vehiculo = new ML.Vehiculo();
                            vehiculo.NumeroReclamo = obj.NumeroReclamo;
                            vehiculo.FechaReclamo = obj.FechaReclamo.Value.ToString("dd-MM-yyyy");
                            vehiculo.HoraPercanse =obj.HoraPercanse.Value;
                            vehiculo.TipoPercanse = obj.TipoPercanse;
                            vehiculo.NumeroPoliza = obj.NumeroPoliza.Value;
                            vehiculo.NombreConductor = obj.NombreConductor;
                            vehiculo.ContactoConductor = obj.ContactoConductor.Value;
                            vehiculo.DetallesVehiculo = obj.DetallesVehiculo;
                            vehiculo.DañosPrejuicios = obj.DañosPrejuicios;
                            vehiculo.EstimacionReparacion = obj.EstimacionReparacion.Value;

                            result.Objects.Add(vehiculo);
                            result.Correct = true;
                        }
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error la tabla no contiene informacion ";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result AddEF(ML.Vehiculo vehiculo)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.VehiculosEntities context = new DL.VehiculosEntities())
                {
                    var resultQuery = context.VehiculoAdd(vehiculo.FechaReclamo, vehiculo.HoraPercanse, vehiculo.TipoPercanse, vehiculo.NumeroPoliza,
                    vehiculo.NombreConductor, vehiculo.ContactoConductor, vehiculo.DetallesVehiculo, vehiculo.DañosPrejuicios, vehiculo.EstimacionReparacion);

                    if (resultQuery > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se puede añadir la direccion " + vehiculo.NumeroReclamo;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;

        }
    }
}
