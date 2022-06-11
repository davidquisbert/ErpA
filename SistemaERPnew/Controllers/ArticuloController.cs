using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;
using Entidad.Estados;
using Microsoft.Reporting.WebForms;

namespace SistemaERPnew.Controllers
{
    public class ArticuloController : Controller
    {
        private LArticulo logica = LArticulo.Logica.LArticulo;
        public ActionResult Index()
        {

            try
            {
                //Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.Categoria = LCategoria.Logica.LCategoria.listarCategoriasArticulos(empresa.IdEmpresa);
                List<Articulo> periodos = new List<Articulo>();
                periodos = logica.listarArticulo(empresa.IdEmpresa);

                return View(periodos);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }

        public ActionResult Listar()
        {

            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                List<Articulo> periodos = new List<Articulo>();
                periodos = logica.listarArticulo(empresa.IdEmpresa);
                return PartialView("ListaArticulo", periodos);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400, ex.Message.Replace("'", ""));
            }


        }
        [HttpPost]
        public ActionResult Registro(string nombre, string descripcion, double precioventa, List<ECategoriaJSON> categorias)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                Articulo periodo = new Articulo()
                {
                    Nombre = nombre,
                    Descripcion = descripcion,
                    PrecioVenta = precioventa,
                    Cantidad = 0,
                    IdEmpresa = empresa.IdEmpresa,
                    IdUsuario = usuario.IdUsuario

                };
                logica.Registro(periodo,categorias);
                //List<Empresa> empresas = new List<Empresa>();
                // empresas = logica.listarEmpresa(usuario.idUsuario);
                // return PartialView("_ListaEmpresa", empresas);
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
        public ActionResult ObtenerArticulo(int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticulo articulo = logica.obtenerArticulo(idarticulo, empresa.IdEmpresa);



                return Json(new
                {
                    Data = articulo

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
        public ActionResult ObtenerLote(int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLote(idarticulo, empresa.IdEmpresa);

                return Json(new
                {
                    Data = lote
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
        public ActionResult ObtenerDatosLote(int nrolote,int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLoteArticulo(nrolote,idarticulo);

                return Json(new
                {
                    Data = lote
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
        public ActionResult ObtenerLoteActivo(int idarticulo)
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERArticuloLote lote = logica.obtenerLoteActivo(idarticulo, empresa.IdEmpresa);

                return Json(new
                {
                    Data = lote
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
        public ActionResult Editar(int id, string nombre, string descripcion, double precioventa, List<ECategoriaJSON> categorias)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Articulo periodo = new Articulo()
                {
                    IdArticulo = id,
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Cantidad = 0,
                    PrecioVenta = precioventa,
                    IdEmpresa = empresa.IdEmpresa,
                    IdUsuario = usuario.IdUsuario

                };
                logica.Editar(periodo,categorias);

                return JavaScript("MostrarMensajeExito('Modificación Exitosa');");
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
        public ActionResult Eliminar(int id)
        {
            try
            {
                Empresa empresa = (Empresa)Session["Empresa"];
                logica.Eliminar(id,empresa.IdEmpresa);
                // string mensaje = "Registro Exitoso";

                return JavaScript("MostrarMensajeEliminacion('Eliminación Exitosa');");

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

       /* public ActionResult ReportePeriodo()
        {
            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                Gestion gestion = (Gestion)Session["Gestion"];
                List<ERPeriodo> periodos = new List<ERPeriodo>();
                periodos = logica.ReportePeriodo(gestion.IdGestion);

                List<ERDatosBasicoPeriodo> datosBasico = new List<ERDatosBasicoPeriodo>();
                datosBasico = logica.ReporteDatosBasicoPeriodo(usuario.Usuario1, empresa.Nombre, gestion.Nombre);
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;
                viewer.Width = 400;
                viewer.Height = 800;

                ReportDataSource rp = new ReportDataSource("DSReportePeriodo", periodos);
                ReportDataSource rb = new ReportDataSource("DSReporteBasicoPeriodo", datosBasico);
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReportePeriodo.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteGestion = viewer;


                return View("ReportePeriodo");
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