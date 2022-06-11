using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidad;
using Logica;
using Datos;
using Entidad.Estados;
using Microsoft.Reporting.WebForms;

namespace SistemaERPnew.Controllers
{
    public class ComprobanteController : Controller
    {
        private LComprobante logica = LComprobante.Logica.LComprobante;

      
        public ActionResult Index()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                return View(logica.listarComprobante(empresa.IdEmpresa));
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }


        public ActionResult Comprobante()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Session["DetalleComprobante"] = new List<EDetalleComprobante>();
                ViewBag.CuentaDetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(empresa.IdEmpresa);
                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Serie = logica.obtenerSerie(empresa.IdEmpresa);

                DetalleEstado d = new DetalleEstado();
                d.Estado = 1;
                Session["EstadoComprobante"] = d;

                EDetalleTotal e = new EDetalleTotal();
                e.TotalDebe = 0;
                e.TotalHaber = 0;

                Session["DetalleTotal"] = e;

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }

        public ActionResult EComprobante(int idcomprobante)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Comprobante comprobante = logica.ObtenerComprobante(empresa.IdEmpresa, idcomprobante);
                ViewBag.CuentaDetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(empresa.IdEmpresa);
                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);



                List<EDetalleComprobante> detalleComprobantes = new List<EDetalleComprobante>();
                detalleComprobantes = logica.listarDetalleComprobanteXComprobante(comprobante.IdComprobante, comprobante.IdEmpresa);

                Session["DetalleComprobante"] = detalleComprobantes;

                DetalleEstado d = new DetalleEstado();
                d.Estado = comprobante.Estado;
                Session["EstadoComprobante"] = d;

                EDetalleTotal e = new EDetalleTotal();
                e.TotalDebe = 0;
                e.TotalHaber = 0;

                foreach (var i in detalleComprobantes)
                {
                    e.TotalDebe = e.TotalDebe + i.Debe;
                    e.TotalHaber = e.TotalHaber + i.Haber;
                }

                Session["DetalleTotal"] = e;

                return View(comprobante);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }

        public ActionResult RegistroDetalleComprobante(int idcuenta, string detalleglosa, double debe, double haber)
        {

            List<EDetalleComprobante> detalle = new List<EDetalleComprobante>();
            EDetalleTotal total = new EDetalleTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                

                var cuenta = LCuenta.Logica.LCuenta.obtenerCuenta(idcuenta);

                if (Double.IsNaN(debe))
                {
                    throw new MensageException("Debe ingresar un valor");
                }

                if (Double.IsNaN(haber))
                {
                    throw new MensageException("Debe ingresar un valor");
                }


                int contador = detalle.Count;
                if (detalle.Count == 0)
                {
                    EDetalleComprobante d = new EDetalleComprobante();
                    d.IdDetalle = contador + 1;
                    d.IdCuenta = cuenta.IdCuenta;
                    d.Cuenta = cuenta.Codigo + " " + cuenta.Nombre;
                    d.Glosa = detalleglosa;
                    d.Debe = debe;
                    d.Haber = haber;
                    detalle.Add(d);

                    total.TotalDebe = debe;
                    total.TotalHaber = haber;

                }
                else if (detalle.Count > 0)
                {

                    foreach (var i in detalle)
                    {
                        if (cuenta.IdCuenta == i.IdCuenta)
                        {
                            throw new MensageException("La cuenta ya existe en el listado");
                        }

                        total.TotalDebe = total.TotalDebe + i.Debe;
                        total.TotalHaber = total.TotalHaber + i.Haber;

                    }
                    var contador2 = detalle.Last();
                    EDetalleComprobante d = new EDetalleComprobante();
                    d.IdDetalle = contador2.IdDetalle + 1;
                    d.IdCuenta = cuenta.IdCuenta;
                    d.Cuenta = cuenta.Codigo + " " + cuenta.Nombre;
                    d.Glosa = detalleglosa;
                    d.Debe = debe;
                    d.Haber = haber;
                    detalle.Add(d);

                    total.TotalDebe = total.TotalDebe + debe;
                    total.TotalHaber = total.TotalHaber + haber;
                }


                total.TotalDebe = total.TotalDebe;
                total.TotalHaber = total.TotalHaber;

                

                Session["DetalleComprobante"] = detalle;
                Session["DetalleTotal"] = total;

                string msj = "Detalle Registrado";
                return JavaScript("MensajeExitoDetalle('" + msj + "');");
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


        public ActionResult ListarDetalleComprobante()
        {

            List<EDetalleComprobante> detalle = new List<EDetalleComprobante>();
            EDetalleTotal total = new EDetalleTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                total = Session["DetalleTotal"] as EDetalleTotal;



                return PartialView("ListaDetalleComprobante", detalle);

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
        public ActionResult VistasDetalleComprobante()
        {

            List<EDetalleComprobante> detalle = new List<EDetalleComprobante>();
            EDetalleTotal total = new EDetalleTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                total = Session["DetalleTotal"] as EDetalleTotal;



                return PartialView("VistaDetalleComprobante", detalle);

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
        public ActionResult EliminarDetalleComprobante(int iddetalle)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<EDetalleComprobante> detalleComprobantes = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                detalleComprobantes.RemoveAll(p => p.IdDetalle == iddetalle);
                EDetalleTotal total = new EDetalleTotal();
                total.TotalDebe = 0;
                total.TotalHaber = 0;
                foreach (var i in detalleComprobantes)
                {

                    total.TotalDebe = total.TotalDebe + i.Debe;
                    total.TotalHaber = total.TotalHaber + i.Haber;

                }


                Session["DetalleComprobante"] = detalleComprobantes;
                Session["DetalleTotal"] = total;

                string msj = "Detalle eliminado";
                return JavaScript("MensajeEliminarDetalle('" + msj + "');");
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

        public ActionResult EditarDetalleComprobante(int idcuentaantigua,int iddetalle,int idcuenta, string detalleglosa, double debe, double haber)
        {

            List<EDetalleComprobante> detalle = new List<EDetalleComprobante>();
            EDetalleTotal total = new EDetalleTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                

                var cuenta = LCuenta.Logica.LCuenta.obtenerCuenta(idcuenta);


                int cuentaRepetidas = 0;

                foreach (var i in detalle)
                {
                    if(idcuentaantigua != idcuenta)
                    {
                        if (i.IdCuenta == idcuenta)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }
                    
                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("La cuenta ya existe en el detalle");
                }


                foreach (var i in detalle)
                {
                    if (i.IdDetalle == iddetalle)
                    {
                        i.IdCuenta = cuenta.IdCuenta;
                        i.Cuenta = cuenta.Codigo + " " + cuenta.Nombre;
                        i.Debe = debe;
                        i.Haber = haber;
                        i.Glosa = detalleglosa;
                    }

                    total.TotalDebe = total.TotalDebe + i.Debe;
                    total.TotalHaber = total.TotalHaber + i.Haber;


                }

                Session["DetalleComprobante"] = detalle;
                Session["DetalleTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalle('" + msj + "');");
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
        public ActionResult Registro(string fecha, int tipocomprobante, double cambio, int moneda, string glosa)
        {

            try
            {
                DateTime FechaComprobante;
                try
                {
                    FechaComprobante = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                int nroserie = logica.obtenerSerie(empresa.IdEmpresa);

                Comprobante Entidad = new Comprobante()
                {
                    Serie = nroserie,
                    Glosa = glosa,
                    Fecha = FechaComprobante,
                    TipoCambio = cambio,
                    Estado = (int)EstadoComprobante.Abierto,
                    TipoComprobante = tipocomprobante,
                    IdEmpresa = empresa.IdEmpresa,
                    IdMoneda = moneda,
                    IdUsuario = usuario.IdUsuario

                };

                List<EDetalleComprobante> detalle = Session["DetalleComprobante"] as List<EDetalleComprobante>;
                EDetalleTotal total = Session["DetalleTotal"] as EDetalleTotal;
                Entidad = logica.Registro(Entidad, detalle, total);

                return JavaScript("redireccionar('" + Url.Action("EComprobante", "Comprobante", new { idcomprobante = Entidad.IdComprobante }) + "');");

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
        public ActionResult Anular(int idcomprobante)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Comprobante Entidad = logica.AnularComprobante(idcomprobante);

               
                return JavaScript("redireccionar('" + Url.Action("EComprobante", "Comprobante", new { idcomprobante = Entidad.IdComprobante }) + "');");

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
        public ActionResult ReporteComprobante(int idcomprobante)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoComprobante(usuario.Usuario1, empresa.Nombre);


                List<EComprobante> comprobante = new List<EComprobante>();
                comprobante = logica.ObtenerComprobanteReporte(empresa.IdEmpresa, idcomprobante);

                List<EDetalleComprobante> detalleComprobantes = new List<EDetalleComprobante>();
                detalleComprobantes = logica.listarDetalleComprobanteXComprobante(idcomprobante, empresa.IdEmpresa);

                List<EDetalleTotal> eDetalleTotal = new List<EDetalleTotal>();
                eDetalleTotal = logica.TotalDetalle(detalleComprobantes);



                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoComprobante", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteComprobante", comprobante);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteDetalleComprobante", detalleComprobantes);
                ReportDataSource rcdetalletotal = new ReportDataSource("DSTotales", eDetalleTotal);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteComprobante.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteComprobante = viewer;


                return View("ReporteComprobante");
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
        

        public ActionResult ListarPeriodo(int idgestion)
        {

            try
            {
                return PartialView("ListarPeriodo", LPeriodo.Logica.LPeriodo.listarPeriodo(idgestion));

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
        public ActionResult ReporteBalanceInicial()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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
        public ActionResult ReporteBalanceGeneral()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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
        public ActionResult ReporteEstadoResultados()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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
        public ActionResult ReporteLibroDiario()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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

        public ActionResult ReporteDeLibroDiario(int idgestion, int idperiodo, int idmoneda)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoComprobante> datosBasico = new List<ERDatosBasicoComprobante>();
                datosBasico = logica.ReporteDatosBasico(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion, idperiodo);



                List<ELibroDiario> detalleComprobantes = new List<ELibroDiario>();
                detalleComprobantes = logica.reporteLibroDiario(idgestion, idperiodo, empresa.IdEmpresa, idmoneda);

                List<EDetalleTotal> eDetalleTotal = new List<EDetalleTotal>();
                eDetalleTotal = logica.TotalLibroDiario(detalleComprobantes);



                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteLibroDiario", detalleComprobantes);
                ReportDataSource rcdetalletotal = new ReportDataSource("DSTotalLibroDiario", eDetalleTotal);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteLibroDiario.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                viewer.LocalReport.DataSources.Add(rcdetalletotal);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteLibroDiario = viewer;


                return PartialView("LibroDiarioParcial");
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
        public ActionResult ReporteDeBalanceInicial(int idgestion, int idmoneda)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                if (empresa.Niveles == 3)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = logica.ReporteDatosBasicoBalanceInicial(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceInicialActivo> detallecomprobantesactivo = new List<EBalanceInicialActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceInicialActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceInicialPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraActivo> cabeceraactivo = new List<ECabeceraActivo>();
                    cabeceraactivo = logica.cabeceraactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabecerapasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicial.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }else if (empresa.Niveles == 4)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = logica.ReporteDatosBasicoBalanceInicial(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceInicialActivo> detallecomprobantesactivo = new List<EBalanceInicialActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceInicialActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceInicialPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraActivo> cabeceraactivo = new List<ECabeceraActivo>();
                    cabeceraactivo = logica.cabeceraactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabecerapasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel4.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }else if (empresa.Niveles == 5)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = logica.ReporteDatosBasicoBalanceInicial(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceInicialActivo> detallecomprobantesactivo = new List<EBalanceInicialActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceInicialActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceInicialPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraActivo> cabeceraactivo = new List<ECabeceraActivo>();
                    cabeceraactivo = logica.cabeceraactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabecerapasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel5.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }else if (empresa.Niveles == 6)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = logica.ReporteDatosBasicoBalanceInicial(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceInicialActivo> detallecomprobantesactivo = new List<EBalanceInicialActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceInicialActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceInicialPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraActivo> cabeceraactivo = new List<ECabeceraActivo>();
                    cabeceraactivo = logica.cabeceraactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabecerapasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel6.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }else if (empresa.Niveles == 7)
                {
                    List<ERDatosBasicoBalanceInicial> datosBasico = new List<ERDatosBasicoBalanceInicial>();
                    datosBasico = logica.ReporteDatosBasicoBalanceInicial(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceInicialActivo> detallecomprobantesactivo = new List<EBalanceInicialActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceInicialActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceInicialPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceInicialPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceInicialPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraActivo> cabeceraactivo = new List<ECabeceraActivo>();
                    cabeceraactivo = logica.cabeceraactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabecerapasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceInicialActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceInicialPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceInicialNivel7.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceInicial = viewer;
                }
               


                return PartialView("BalanceInicialParcial");
            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un erros controlador');");
            }

        }
        public ActionResult ReporteDeBalanceGeneral(int idgestion, int idmoneda)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                if (empresa.Niveles == 3)
                {
                    List<ERDatosBasicoBalanceGeneral> datosBasico = new List<ERDatosBasicoBalanceGeneral>();
                    datosBasico = logica.ReporteDatosBasicoBalanceGeneral(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceGeneralActivo> detallecomprobantesactivo = new List<EBalanceGeneralActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceGeneralActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceGeneralPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceGeneralPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceGeneralPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraGeneralActivo> cabeceraactivo = new List<ECabeceraGeneralActivo>();
                    cabeceraactivo = logica.cabeceraGeneralactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraGeneralPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraGeneralPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabeceraGeneralpasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceGeneralActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceGeneralPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceGeneral.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceGeneral = viewer;
                }
                else if (empresa.Niveles == 4)
                {
                    List<ERDatosBasicoBalanceGeneral> datosBasico = new List<ERDatosBasicoBalanceGeneral>();
                    datosBasico = logica.ReporteDatosBasicoBalanceGeneral(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceGeneralActivo> detallecomprobantesactivo = new List<EBalanceGeneralActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceGeneralActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceGeneralPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceGeneralPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceGeneralPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraGeneralActivo> cabeceraactivo = new List<ECabeceraGeneralActivo>();
                    cabeceraactivo = logica.cabeceraGeneralactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraGeneralPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraGeneralPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabeceraGeneralpasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceGeneralActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceGeneralPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceGeneralNivel4.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceGeneral = viewer;
                }
                else if (empresa.Niveles == 5)
                {
                    List<ERDatosBasicoBalanceGeneral> datosBasico = new List<ERDatosBasicoBalanceGeneral>();
                    datosBasico = logica.ReporteDatosBasicoBalanceGeneral(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceGeneralActivo> detallecomprobantesactivo = new List<EBalanceGeneralActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceGeneralActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceGeneralPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceGeneralPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceGeneralPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraGeneralActivo> cabeceraactivo = new List<ECabeceraGeneralActivo>();
                    cabeceraactivo = logica.cabeceraGeneralactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraGeneralPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraGeneralPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabeceraGeneralpasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceGeneralActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceGeneralPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceGeneralNivel5.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceGeneral = viewer;
                }
                else if (empresa.Niveles == 6)
                {
                    List<ERDatosBasicoBalanceGeneral> datosBasico = new List<ERDatosBasicoBalanceGeneral>();
                    datosBasico = logica.ReporteDatosBasicoBalanceGeneral(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceGeneralActivo> detallecomprobantesactivo = new List<EBalanceGeneralActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceGeneralActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceGeneralPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceGeneralPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceGeneralPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraGeneralActivo> cabeceraactivo = new List<ECabeceraGeneralActivo>();
                    cabeceraactivo = logica.cabeceraGeneralactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraGeneralPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraGeneralPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabeceraGeneralpasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceGeneralActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceGeneralPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceGeneralNivel6.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceGeneral = viewer;
                }
                else if (empresa.Niveles == 7)
                {
                    List<ERDatosBasicoBalanceGeneral> datosBasico = new List<ERDatosBasicoBalanceGeneral>();
                    datosBasico = logica.ReporteDatosBasicoBalanceGeneral(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);


                    List<EBalanceGeneralActivo> detallecomprobantesactivo = new List<EBalanceGeneralActivo>();
                    detallecomprobantesactivo = logica.reporteBalanceGeneralActivo(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EBalanceGeneralPasivoPatrimonio> detallecomprobantepasivopatrimonio = new List<EBalanceGeneralPasivoPatrimonio>();
                    detallecomprobantepasivopatrimonio = logica.reporteBalanceGeneralPasivoPatrimonio(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraGeneralActivo> cabeceraactivo = new List<ECabeceraGeneralActivo>();
                    cabeceraactivo = logica.cabeceraGeneralactivos(idgestion, empresa.IdEmpresa, idmoneda);


                    List<ECabeceraGeneralPasivoPatrimonio> cabecerapasivopatrimonio = new List<ECabeceraGeneralPasivoPatrimonio>();
                    cabecerapasivopatrimonio = logica.cabeceraGeneralpasivopatrimonios(idgestion, empresa.IdEmpresa, idmoneda);

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleactivo = new ReportDataSource("DSReporteBalanceGeneralActivo", detallecomprobantesactivo);
                    ReportDataSource rcdetallepasivopatrimonio = new ReportDataSource("DSReporteBalanceGeneralPasivoPatrimonio", detallecomprobantepasivopatrimonio);
                    ReportDataSource rccabeceraactivo = new ReportDataSource("DSCabecera1", cabeceraactivo);
                    ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteBalanceGeneralNivel7.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleactivo);
                    viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraactivo);
                    viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteBalanceGeneral = viewer;
                }



                return PartialView("BalanceGeneralParcial");
            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un erros controlador');");
            }

        }
        public ActionResult ReporteDeEstadoResultados(int idgestion, int idmoneda)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                    List<ERDatosBasicoEstadoResultados> datosBasico = new List<ERDatosBasicoEstadoResultados>();
                    datosBasico = logica.ReporteDatosBasicoEstadoResultados(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);
                   

                    List<EEstadoResultadosIngreso> detallecomprobantesingreso = new List<EEstadoResultadosIngreso>();
                    detallecomprobantesingreso = logica.reporteestadoresultadosingreso(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EEstadoResultadosCosto> detallecomprobantecosto = new List<EEstadoResultadosCosto>();
                    detallecomprobantecosto = logica.reporteestadoresultadoscosto(idgestion, empresa.IdEmpresa, idmoneda);

                    List<EEstadoResultadosGasto> detallecomprobantegasto = new List<EEstadoResultadosGasto>();
                    detallecomprobantegasto = logica.reporteestadoresultadosgasto(idgestion, empresa.IdEmpresa, idmoneda);
                    
                    List<ECabeceraEstadoResultadosIngreso> cabeceraingreso = new List<ECabeceraEstadoResultadosIngreso>();
                    cabeceraingreso = logica.cabeceraestadoresultadosingreso(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraEstadoResultadosCosto> cabeceracosto = new List<ECabeceraEstadoResultadosCosto>();
                    cabeceracosto = logica.cabeceraestadoresultadoscosto(idgestion, empresa.IdEmpresa, idmoneda);

                    List<ECabeceraEstadoResultadosGasto> cabeceragasto = new List<ECabeceraEstadoResultadosGasto>();
                    cabeceragasto = logica.cabeceraestadoresultadosgasto(idgestion, empresa.IdEmpresa, idmoneda);
                    

                    ReportViewer viewer = new ReportViewer();
                    viewer.AsyncRendering = false;
                    viewer.SizeToReportContent = true;

                    ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                    ReportDataSource rcdetalleingreso = new ReportDataSource("DSReporteEstadoResultadosIngreso", detallecomprobantesingreso);
                    ReportDataSource rcdetallecosto = new ReportDataSource("DSReporteEstadoResultadosCosto", detallecomprobantecosto);
                    ReportDataSource rcdetallegasto = new ReportDataSource("DSReporteEstadoResultadosGasto", detallecomprobantegasto);
                    ReportDataSource rccabeceraingreso = new ReportDataSource("DSCabecera1", cabeceraingreso);
                    ReportDataSource rccabeceracosto = new ReportDataSource("DSCabecera2", cabeceracosto);
                    ReportDataSource rccabeceragasto = new ReportDataSource("DSCabecera3", cabeceragasto);
                    /*ReportDataSource rccabecerapasivopatrimonio = new ReportDataSource("DSCabecera2", cabecerapasivopatrimonio);*/

                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteEstadoResultados.rdlc");
                    viewer.LocalReport.DataSources.Clear();

                    viewer.LocalReport.DataSources.Add(rb);
                    viewer.LocalReport.DataSources.Add(rcdetalleingreso);
                    viewer.LocalReport.DataSources.Add(rcdetallecosto);
                    viewer.LocalReport.DataSources.Add(rcdetallegasto);
                    // viewer.LocalReport.DataSources.Add(rcdetallepasivopatrimonio);
                    viewer.LocalReport.DataSources.Add(rccabeceraingreso);
                    viewer.LocalReport.DataSources.Add(rccabeceracosto);
                    viewer.LocalReport.DataSources.Add(rccabeceragasto);
                    //viewer.LocalReport.DataSources.Add(rccabecerapasivopatrimonio);

                    viewer.LocalReport.Refresh();

                    ViewBag.ReporteEstadoResultados = viewer;
              


                return PartialView("EstadoResultadosParcial");
            }
            catch (MensageException ex)
            {
                string mensaje = ex.Message.Replace("'", "");
                ViewBag.Mensaje = mensaje;
                return JavaScript("MostrarMensaje('" + mensaje + "');");
            }
            catch (Exception ex)
            {

                return JavaScript("MostrarMensaje('Ha ocurrido un erros controlador');");
            }

        }
        public ActionResult ReporteDeSumasySaldos(int idgestion, int idmoneda)
        {
            try
            {
                ParametroIdgestion = idgestion;
                ParametroIdmoneda = idmoneda;

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoSumasySaldos> datosBasico = new List<ERDatosBasicoSumasySaldos>();
                datosBasico = logica.ReporteDatosBasicoSumasySaldos(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion);
               
                List<ESumasySaldos> libroMayores = new List<ESumasySaldos>();

                List<ESumasySaldos> libroMayors = new List<ESumasySaldos>();

                List<ELibroMayoCabezera> detalleComprobantes = new List<ELibroMayoCabezera>();
                detalleComprobantes = logica.reporteSumasySaldosCabezera(idgestion, empresa.IdEmpresa);

                foreach (var i in detalleComprobantes)
                {
                    libroMayors = logica.reporteSumasySaldos(i.IdCuenta, idgestion, empresa.IdEmpresa, idmoneda);
                    libroMayores.AddRange(libroMayors);
                }
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSCabezeraSumasySaldos", libroMayores);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteSumasySaldos.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteSumasySaldos = viewer;


                return PartialView("SumasySaldosParcial");
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
        public ActionResult ReporteSumasySaldos()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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
        public ActionResult ReporteLibroMayor()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                ViewBag.EmpresaMonedas = LMoneda.Logica.LMoneda.listarMonedaActivaXEmpresa(empresa.IdEmpresa);
                ViewBag.Gestion = LGestion.Logica.LGestion.listarGestion(empresa.IdEmpresa, usuario.IdUsuario);

                return View();
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
        int ParametroIdgestion = 0;
        int ParametroIdPeriodo = 0;
        int ParametroIdmoneda = 0;
        int ParametroIdCuenta = 0;

        public ActionResult ReporteDeLibroMayor(int idgestion, int idperiodo, int idmoneda)
        {
            try
            {
                ParametroIdgestion = idgestion;
                ParametroIdmoneda = idmoneda;
                ParametroIdPeriodo = idperiodo;

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoComprobante> datosBasico = new List<ERDatosBasicoComprobante>();
                datosBasico = logica.ReporteDatosBasico(usuario.Usuario1, empresa.Nombre, idmoneda, idgestion, idperiodo);

                List<ELibroMayor> libroMayores = new List<ELibroMayor>();

                List<ELibroMayor> libroMayors = new List<ELibroMayor>();

                List<ELibroMayoCabezera> detalleComprobantes = new List<ELibroMayoCabezera>();
                detalleComprobantes = logica.reporteLibroMayorCabezera(idgestion, idperiodo, empresa.IdEmpresa);

                foreach (var i in detalleComprobantes)
                {
                    libroMayors = logica.reporteLibroMayor(i.IdCuenta, idgestion, idperiodo, empresa.IdEmpresa, idmoneda);
                    libroMayores.AddRange(libroMayors);
                }
                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasico", datosBasico);
                ReportDataSource rcdetalle = new ReportDataSource("DSCabezeraLibro", libroMayores);

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteLibroMayor.rdlc");
                viewer.LocalReport.DataSources.Clear();

                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);
                
                viewer.LocalReport.Refresh();

                ViewBag.ReporteLibroMayor = viewer;


                return PartialView("LibroMayorParcial");
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