using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class VehiculoController : Controller
    {
        // GET: Vehiculo
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Vehiculo vehiculo = new ML.Vehiculo();
            vehiculo.Vehiculos = new List<object>();
            ML.Result result = BL.Vehiculo.GetAllEF();


            if (result.Correct)
            {
                vehiculo.Vehiculos = result.Objects;
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;
            }
            return View(vehiculo);
        }

        [HttpGet]
        public ActionResult UploadData()
        {
            ML.Vehiculo vehiculo = new ML.Vehiculo();

            return View(vehiculo);
        }

        [HttpPost]
        public ActionResult UploadData(ML.Vehiculo vehiculo, HttpPostedFileBase cargaMasiva)
        {
          // string fileExtension = Path.GetExtension(cargaMasiva.FileName);
            if (Session["ListVehiculos"] == null)
            {
                string conexionExel = System.Configuration.ConfigurationManager.AppSettings["ConexionExcel"];

                string direccionExcel = Server.MapPath("~/CargaMasiva/Excel/") + Path.GetFileNameWithoutExtension(cargaMasiva.FileName) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                try
                {
                    cargaMasiva.SaveAs(direccionExcel);
                    vehiculo = PrevisualizarExcel(vehiculo, direccionExcel, conexionExel);
                    Session["ListVehiculos"] = vehiculo.Vehiculos;


                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                var lista = (List<Object>)Session["ListVehiculos"];
                vehiculo.Vehiculos = lista;
                CargaDatos(vehiculo);
                Session.Remove("ListVehiculos");
                ViewBag.Message = "Datos cargados ";
                return PartialView("Modal");

            }

            //}
            return View(vehiculo);
        }

        public static ML.Vehiculo PrevisualizarExcel(ML.Vehiculo vehiculo, string rutaExel, string conexion)
        {
            using (OleDbConnection file = new OleDbConnection(conexion + rutaExel))
            {
                try
                {
                    string query = "SELECT * FROM [Hoja 1$]";
                    using (OleDbCommand context = new OleDbCommand())
                    {
                        context.CommandText = query;
                        context.Connection = file;

                        OleDbDataAdapter data = new OleDbDataAdapter(context);
                        data.SelectCommand = context;

                        DataTable tableEmpleado = new DataTable();
                        data.Fill(tableEmpleado);

                        if (tableEmpleado.Rows.Count > 0)
                        {

                            vehiculo.Vehiculos = new List<object>();

                            foreach (DataRow row in tableEmpleado.Rows)
                            {

                                ML.Vehiculo vehiculoItem = new ML.Vehiculo();
                                vehiculoItem.FechaReclamo = row[0].ToString();
                                vehiculoItem.HoraPercanse = int.Parse(row[1].ToString());
                                vehiculoItem.TipoPercanse = row[2].ToString();
                                vehiculoItem.NumeroPoliza = int.Parse(row[3].ToString());
                                vehiculoItem.NombreConductor = row[4].ToString();
                                vehiculoItem.ContactoConductor = int.Parse(row[5].ToString());
                                vehiculoItem.DetallesVehiculo = row[6].ToString();
                                vehiculoItem.DañosPrejuicios = row[7].ToString();
                                vehiculoItem.EstimacionReparacion = decimal.Parse(row[8].ToString());

                                vehiculo.Vehiculos.Add(vehiculoItem);
                            }
                        }
                    }
                }
                catch (IOException ex)
                {

                }
            }
            return vehiculo;

        }

        public void CargaDatos(ML.Vehiculo vehiculo)
        {

            List<string> resultLines = new List<string>();

            foreach (ML.Vehiculo vehiculoItem in vehiculo.Vehiculos)
            {
                ML.Result result = BL.Vehiculo.AddEF(vehiculoItem);
                if (result.Correct)
                {
                    resultLines.Add("La inserccion del Empleado" + vehiculo.NumeroPoliza + "Fue exitoso");
                    result.Correct = true;
                }
                else
                {
                    resultLines.Add("Hubo un erro al actualizar de Empleado" + vehiculo.NumeroPoliza + "Error" + result.ErrorMessage);
                    result.Correct = false;

                }
            }

            var fecha = DateTime.Now.ToString("dd-MM-yyyy HHmmss");
            string path = Server.MapPath("~/CargaMasiva/EXCEL/" + "CargaMasivaEXCEL_" + fecha + ".txt");

            using (StreamWriter archvioError = System.IO.File.CreateText(path))
            {
                foreach (string line in resultLines)
                {
                    archvioError.WriteLine(line);
                }
            }
        }
    }
}