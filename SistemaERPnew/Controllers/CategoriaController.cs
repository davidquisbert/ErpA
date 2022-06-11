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
    public class CategoriaController : Controller
    {
        // GET: Categoria
        private LCategoria logica = LCategoria.Logica.LCategoria;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListarCategoria()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ECategoria> categorias = new List<ECategoria>();
                categorias = logica.listarCategorias(empresa.IdEmpresa);



                return Json(new
                {
                    Categorias = categorias

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
        public ActionResult ObtenerCategoria(int idcategoria)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                //List<ECuenta> cuentas = new List<ECuenta>();
                var entidad = logica.obtenerCategoria(idcategoria);


                return Json(new
                {
                    Nombre = entidad.Nombre,
                    Descripcion = entidad.Descripcion

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
        public ActionResult Registro(string nombre,string descripcion, int idCategoria, int idPadre)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                if (idPadre == 0)
                {
                    //hijo 0
                    Categoria categoria = new Categoria()
                    {
                        Nombre = nombre,
                        Descripcion = descripcion,
                        IdUsuario = usuario.IdUsuario,
                        IdEmpresa = empresa.IdEmpresa,
                        IdCategoriaPadre = idCategoria


                    };
                    logica.Registro(categoria, idCategoria, 0);
                }

                else if (idPadre == 1)
                {
                    //padre 1
                    Categoria categoria = new Categoria()
                    {
                        Nombre = nombre,
                        Descripcion = descripcion,
                        IdUsuario = usuario.IdUsuario,
                        IdEmpresa = empresa.IdEmpresa,



                    };
                    logica.Registro(categoria, 0, 1);
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
        public ActionResult Editar(string nombre, string descripcion, int idcategoria)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];



                logica.ModificarCategoria(idcategoria, nombre, descripcion, empresa.IdEmpresa);


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
        public ActionResult Eliminar(int idcategoria)
        {

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                logica.EliminarCategoria(idcategoria);

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
        /* public ActionResult ReporteCuenta()
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

         }*/
    }
}