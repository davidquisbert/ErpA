using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad.Estados;
using Entidad;
using Microsoft.Reporting.WebForms;

namespace SistemaERPnew.Controllers
{
    public class CuentaController : Controller
    {

        private LCuenta logica = LCuenta.Logica.LCuenta;
        public ActionResult Index()
        {


            return View();
        }

        [HttpPost]
        public ActionResult ListarCuenta()
        {

            /* Usuario usuario = (Usuario)Session["Usuario"];
             Empresa empresa = (Empresa)Session["Empresa"];



             return View();*/

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ECuenta> cuentas = new List<ECuenta>();
                cuentas = logica.listarCuentas(usuario.IdUsuario, empresa.IdEmpresa);

            

                return Json(new
                {
                    Cuentas = cuentas

                });

            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }






        [HttpPost]
        public ActionResult ObtenerCuenta(int idcuenta)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                //List<ECuenta> cuentas = new List<ECuenta>();
                var entidad = logica.obtenerCuenta(idcuenta);


                return Json(new
                {
                    Nombre = entidad.Nombre

                });

            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }




        [HttpPost]
        public ActionResult Registro(string nombre, int idCuenta, int idPadre)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                if (idPadre == 0)
                {
                    //hijo 0
                    Cuenta cuenta = new Cuenta()
                    {
                        Nombre = nombre,
                        Codigo = "",
                        Nivel = 0,
                        TipoDeCuenta = (int)TipoCuenta.Global,
                        IdUsuario = usuario.IdUsuario,
                        IdEmpresa = empresa.IdEmpresa,
                        IdCuentaPadre = idCuenta


                    };
                    logica.Registro(cuenta, idCuenta, 0);
                }

                else if (idPadre == 1)
                {
                    //padre 1
                    Cuenta cuenta = new Cuenta()
                    {
                        Nombre = nombre,
                        Codigo = "",
                        Nivel = 0,
                        TipoDeCuenta = (int)TipoCuenta.Global,
                        IdUsuario = usuario.IdUsuario,
                        IdEmpresa = empresa.IdEmpresa,



                    };
                    logica.Registro(cuenta, 0, 1);
                }

                return JavaScript("MostrarMensajeExito('Registro Exitoso');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }

        }


        [HttpPost]
        public ActionResult Editar(string nombre, int idCuenta)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];



                logica.ModificarCuenta(idCuenta, nombre, empresa.IdEmpresa);

  
                return JavaScript("MostrarMensajeExitoEditar('Modificacion Exitoso');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }

        }


        [HttpPost]
        public ActionResult Eliminar(int idCuenta)
        {

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                logica.EliminarCuenta(idCuenta);

                return JavaScript("MostrarMensajeEliminacion('Eliminación Exitosa');");
            }
            catch (MensageException ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('" + ex.Message + "');");
            }

        }
        public ActionResult ReporteCuenta()
        {
            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                List<ECuenta> cuentas = new List<ECuenta>();
                cuentas = logica.listarCuentasReporte(usuario.IdUsuario, empresa.IdEmpresa);

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoCuenta(usuario.Usuario1, empresa.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rp = new ReportDataSource("DSReporteCuenta", cuentas);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoCuenta", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteCuenta.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteCuenta = viewer;


                return View("ReporteCuenta");
            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un error');");
            }

        }

    }
}