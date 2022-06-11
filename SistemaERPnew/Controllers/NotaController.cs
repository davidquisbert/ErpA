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
    public class NotaController : Controller
    {
        // GET: Nota
        private LNota logica = LNota.Logica.LNota;


        public ActionResult Index()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                return View(logica.listarNota(empresa.IdEmpresa));
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error CONTROLADOR.');");
            }
        }
        public ActionResult Index2()
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                return View(logica.listarNota2(empresa.IdEmpresa));
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error CONTROLADOR.');");
            }
        }
        public ActionResult Nota()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Session["DetalleNota"] = new List<ELote>();
                ViewBag.Articulos = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);
                ViewBag.NroNotaCompra = logica.obtenerNroNotaCompra(empresa.IdEmpresa);

                NotaEstado d = new NotaEstado();
                d.Estado = 1;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;

                Session["NotaTotal"] = e;

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult NotaVenta()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                
                Session["DetalleNotaVenta"] = new List<EDetalle>();
                ViewBag.ArticulosConLotes = LArticulo.Logica.LArticulo.listarArticuloConLotes(empresa.IdEmpresa);
                ViewBag.NroNotaVenta = logica.obtenerNroNotaVenta(empresa.IdEmpresa);

                NotaEstado d = new NotaEstado();
                d.Estado = 1;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;

                Session["NotaTotal"] = e;

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult ENota(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota nota = logica.ObtenerNota(empresa.IdEmpresa, idnota);
                ViewBag.Articulo = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);

                List<ELote> detalleComprobantes = new List<ELote>();
                detalleComprobantes = logica.listarLoteXNota(nota.IdNota, empresa.IdEmpresa);

                Session["DetalleNota"] = detalleComprobantes;

                NotaEstado d = new NotaEstado();
                d.Estado = nota.Estado;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;
                if (nota.Tipo == (int)TipoNota.Compra)
                {
                    foreach (var i in detalleComprobantes)
                    {
                        e.Total = e.Total + i.SubTotal;
                    }
                }

                Session["NotaTotal"] = e;

                return View(nota);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult ENotaVenta(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota nota = logica.ObtenerNota(empresa.IdEmpresa, idnota);
                ViewBag.Articulo = LArticulo.Logica.LArticulo.listarArticulo(empresa.IdEmpresa);

                List<EDetalle> detalleComprobantesVenta = new List<EDetalle>();
                detalleComprobantesVenta = logica.listarDetalleXNota(nota.IdNota, empresa.IdEmpresa);

                Session["DetalleNotaVenta"] = detalleComprobantesVenta;

                NotaEstado d = new NotaEstado();
                d.Estado = nota.Estado;
                Session["EstadoNota"] = d;

                ENotaTotal e = new ENotaTotal();
                e.Total = 0;
                 if (nota.Tipo == (int)TipoNota.Venta)
                {
                    foreach (var i in detalleComprobantesVenta)
                    {
                        e.Total = e.Total + i.SubTotal;
                    }
                }

                Session["NotaTotal"] = e;

                return View(nota);
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult RegistroLote(int idarticulo, string fechavencimiento, int cantidad, double preciocompra, double subtotal)
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo,empresa.IdEmpresa);

                if (Double.IsNaN(preciocompra))
                {
                    throw new MensageException("Debe ingresar un precio de compra");
                }


                int contador = 0;
                contador = detalle.Count();

                if (detalle.Count() == 0)
                {
                    ELote d = new ELote();
                    d.idlote = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.FechaVencimiento = fechavencimiento;
                    d.Cantidad = cantidad;
                    d.PrecioCompra = preciocompra;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total+subtotal;

                }
                else if (detalle.Count() > 0)
                {

                    foreach (var i in detalle)
                    {
                        if (articulo.IdArticulo == i.IdArticulo)
                        {
                            throw new MensageException("El articulo ya existe en el listado");
                        }

                        total.Total = total.Total + i.SubTotal;
                        
                    }
                    var contador2 = detalle.Last();
                    ELote d = new ELote();
                    d.idlote = contador2.idlote+1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.FechaVencimiento = fechavencimiento;
                    d.Cantidad = cantidad;
                    d.PrecioCompra = preciocompra;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total + subtotal;
                }


                total.Total = total.Total;



                Session["DetalleNota"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Articulo Registrado";
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
        public ActionResult RegistroDetalle(int idarticulo, int idlote, int cantidad, double precioventa, double subtotal,int stock)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if(cantidad > stock)
                {
                    throw new MensageException("La cantidad debe ser menor o igual al stock");
                }


                int contador = 0;
                contador = detalle.Count();

                if (detalle.Count() == 0)
                {
                    EDetalle d = new EDetalle();
                    d.iddetalle = contador + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.NroLote = idlote;
                    d.Articulo = articulo.NombreArticulo;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total + subtotal;

                }
                else if (detalle.Count() > 0)
                {

                    foreach (var i in detalle)
                    {
                        if (articulo.IdArticulo == i.IdArticulo && i.NroLote==idlote)
                        {
                            throw new MensageException("Ya existe este articulo con el mismo lote en la lista");
                        }

                        total.Total = total.Total + i.SubTotal;

                    }
                    var contador2 = detalle.Last();
                    EDetalle d = new EDetalle();
                    d.iddetalle = contador2.iddetalle + 1;
                    d.IdArticulo = articulo.IdArticulo;
                    d.Articulo = articulo.NombreArticulo;
                    d.NroLote = idlote;
                    d.Cantidad = cantidad;
                    d.PrecioVenta = precioventa;
                    d.SubTotal = subtotal;
                    detalle.Add(d);

                    total.Total = total.Total + subtotal;
                }


                total.Total = total.Total;



                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Articulo Registrado";
                return JavaScript("MensajeExitoDetalleVenta('" + msj + "');");
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
        public ActionResult ListarLote()
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                total = Session["NotaTotal"] as ENotaTotal;



                return PartialView("ListaLote", detalle);

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
        public ActionResult ListarDetalle()
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;
                total = Session["NotaTotal"] as ENotaTotal;



                return PartialView("ListaDetalle", detalle);

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
        public ActionResult VistasLote()
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;
                total = Session["NotaTotal"] as ENotaTotal;

                return PartialView("VistaLote", detalle);

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
        public ActionResult VistasDetalle()
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;
                total = Session["NotaTotal"] as ENotaTotal;

                return PartialView("VistaDetalle", detalle);

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
        public ActionResult EditarLote(int idlote, int idarticuloantiguo, int idarticulo, string fechavencimiento, int cantidad, double preciocompra,double subtotal)
        {

            List<ELote> detalle = new List<ELote>();
            ENotaTotal total = new ENotaTotal();

            try
            {


                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNota"] as List<ELote>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo,empresa.IdEmpresa);


                int cuentaRepetidas = 0;

                foreach (var i in detalle)
                {
                    if (idarticuloantiguo != idarticulo)
                    {
                        if (i.IdArticulo == idarticulo)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }

                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("El Articulo ya existe en el Lote");
                }


                foreach (var i in detalle)
                {
                    if (i.idlote == idlote)
                    {
                        i.IdArticulo = articulo.IdArticulo;
                        i.Articulo = articulo.NombreArticulo;
                        i.FechaVencimiento = fechavencimiento;
                        i.Cantidad = cantidad;
                        i.PrecioCompra = preciocompra;
                        i.SubTotal = subtotal;
                    }

                    total.Total = total.Total + i.SubTotal;


                }

                Session["DetalleNota"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalleEditar('" + msj + "');");
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
        public ActionResult EditarDetalle(int iddetalle, int idarticuloantiguo, int idarticulo,int idlote, int cantidad, double precioventa, double subtotal,int stock)
        {

            List<EDetalle> detalle = new List<EDetalle>();
            ENotaTotal total = new ENotaTotal();

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                detalle = Session["DetalleNotaVenta"] as List<EDetalle>;


                var articulo = LArticulo.Logica.LArticulo.obtenerArticulo(idarticulo, empresa.IdEmpresa);

                if (Double.IsNaN(precioventa))
                {
                    throw new MensageException("Debe ingresar un precio de Venta");
                }
                if (cantidad > stock)
                {
                    throw new MensageException("La cantidad debe ser menor o igual al stock");
                }

                int cuentaRepetidas = 0;
                int loteRepetido = 0;

                foreach (var i in detalle)
                {
                    if (idarticuloantiguo != idarticulo)
                    {
                        if (i.IdArticulo == idarticulo && i.NroLote==idlote)
                        {
                            cuentaRepetidas = cuentaRepetidas + 1;
                        }
                    }

                    if(idarticuloantiguo==idarticulo)
                    {
                        if (i.IdArticulo == idarticulo && i.NroLote == idlote)
                        {
                            loteRepetido = loteRepetido + 1;
                        }
                    }
                }

                if (cuentaRepetidas >= 1)
                {
                    throw new MensageException("El Articulo con este lote ya existe en el detalle");
                }
                if (loteRepetido > 1)
                {
                    throw new MensageException("El Articulo con este lote ya existe en el detalle");
                }


                foreach (var i in detalle)
                {
                    if (i.iddetalle == iddetalle)
                    {
                        i.IdArticulo = articulo.IdArticulo;
                        i.Articulo = articulo.NombreArticulo;
                        i.NroLote = idlote;
                        i.Cantidad = cantidad;
                        i.PrecioVenta = precioventa;
                        i.SubTotal = subtotal;
                    }

                    total.Total = total.Total + i.SubTotal;


                }

                Session["DetalleNotaVenta"] = detalle;
                Session["NotaTotal"] = total;

                string msj = "Detalle editado";
                return JavaScript("MensajeExitoDetalleEditar('" + msj + "');");
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
        public ActionResult EliminarLote(int idlote)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ELote> detalleComprobantes = Session["DetalleNota"] as List<ELote>;
                detalleComprobantes.RemoveAll(p => p.idlote == idlote);
                ENotaTotal total = new ENotaTotal();
                total.Total = 0;
                foreach (var i in detalleComprobantes)
                {

                    total.Total = total.Total + i.SubTotal;

                }


                Session["DetalleNota"] = detalleComprobantes;
                Session["NotaTotal"] = total;

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
        public ActionResult EliminarDetalle(int iddetalle)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<EDetalle> detalleComprobantes = Session["DetalleNotaVenta"] as List<EDetalle>;
                detalleComprobantes.RemoveAll(p => p.iddetalle == iddetalle);
                ENotaTotal total = new ENotaTotal();
                total.Total = 0;
                foreach (var i in detalleComprobantes)
                {

                    total.Total = total.Total + i.SubTotal;

                }


                Session["DetalleNotaVenta"] = detalleComprobantes;
                Session["NotaTotal"] = total;

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

        [HttpPost]
        public ActionResult Registro(string fecha, int tiponota, string descripcion)
        {

            try
            {
                DateTime FechaNota;
                try
                {
                    FechaNota = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                int nronota = logica.obtenerNroNotaCompra(empresa.IdEmpresa);

                    Nota Entidad = new Nota()
                    {
                        NroNota = nronota,
                        Descripcion = descripcion,
                        Fecha = FechaNota,
                        Estado = (int)EstadoNota.Activo,
                        Tipo = tiponota,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario

                    };
                    List<ELote> detalle = Session["DetalleNota"] as List<ELote>;
                    ENotaTotal total = Session["NotaTotal"] as ENotaTotal;
                    Entidad = logica.Registro(Entidad, detalle, total);
                    
                
                return JavaScript("redireccionar('" + Url.Action("ENota", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult RegistroVenta(string fecha, int tiponota, string descripcion)
        {

            try
            {
                DateTime FechaNota;
                try
                {
                    FechaNota = DateTime.Parse(fecha);

                }
                catch (Exception ex)
                {
                    throw new MensageException("Formato de Fecha invalido");
                }

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                int nronotav = logica.obtenerNroNotaVenta(empresa.IdEmpresa);
               
                    Nota Entidad = new Nota()
                    {
                        NroNota = nronotav,
                        Descripcion = descripcion,
                        Fecha = FechaNota,
                        Estado = (int)EstadoNota.Activo,
                        Tipo = tiponota,
                        IdEmpresa = empresa.IdEmpresa,
                        IdUsuario = usuario.IdUsuario

                    };
                    List<EDetalle> detalleventa = Session["DetalleNotaVenta"] as List<EDetalle>;
                    ENotaTotal total = Session["NotaTotal"] as ENotaTotal;
                    Entidad = logica.RegistroVenta(Entidad, detalleventa, total);
                
                return JavaScript("redireccionar('" + Url.Action("ENotaVenta", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult Anular(int idnota)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota Entidad = logica.AnularNota(idnota);


                return JavaScript("redireccionar('" + Url.Action("ENota", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        public ActionResult AnularVenta(int idnota)
        {

            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                Nota Entidad = logica.AnularNotaVenta(idnota,empresa.IdEmpresa);


                return JavaScript("redireccionar('" + Url.Action("ENotaVenta", "Nota", new { idnota = Entidad.IdNota }) + "');");

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
        //REPORTES
        public ActionResult ReporteNotaCompra(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.Nombre);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotaReporte(empresa.IdEmpresa, idnota);

                List<ELote> detalleComprobantes = new List<ELote>();
                detalleComprobantes = logica.listarDetalleNotaCompraXNota(idnota, empresa.IdEmpresa);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteDetalleNota", detalleComprobantes);
                

                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaCompra.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotaCompra = viewer;


                return View("ReporteNotaCompra");
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
        public ActionResult ReporteNotaVenta(int idnota)
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];

                List<ERDatosBasicoCuenta> datosBasico = new List<ERDatosBasicoCuenta>();
                datosBasico = logica.ReporteDatosBasicoNota(usuario.Usuario1, empresa.Nombre);


                List<ENota> comprobante = new List<ENota>();
                comprobante = logica.ObtenerNotaReporte(empresa.IdEmpresa, idnota);

                List<EDetalle> detalleComprobantes = new List<EDetalle>();
                detalleComprobantes = logica.listarDetalleNotaVentaXNota(idnota, empresa.IdEmpresa);

                ReportViewer viewer = new ReportViewer();
                viewer.AsyncRendering = false;
                viewer.SizeToReportContent = true;

                ReportDataSource rb = new ReportDataSource("DSReporteBasicoNota", datosBasico);
                ReportDataSource rp = new ReportDataSource("DSReporteNota", comprobante);
                ReportDataSource rcdetalle = new ReportDataSource("DSReporteDetalleNota", detalleComprobantes);


                viewer.LocalReport.ReportPath = Server.MapPath("~/Reportes/ReporteNotaVenta.rdlc");
                viewer.LocalReport.DataSources.Clear();
                viewer.LocalReport.DataSources.Add(rp);
                viewer.LocalReport.DataSources.Add(rb);
                viewer.LocalReport.DataSources.Add(rcdetalle);

                viewer.LocalReport.Refresh();

                ViewBag.ReporteNotaVenta = viewer;


                return View("ReporteNotaVenta");
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
        //GRAFICOS ECHART
        [HttpPost]
        public ActionResult ObtenerVentasPorEmpresa()
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERNotasVenta nota = logica.obtenerNotasVenta(empresa.IdEmpresa);



                return Json(new
                {
                    Data = nota

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
        public ActionResult ObtenerProductosBajosPorEmpresa()
        {


            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];


                ERGraficos2 producto = logica.obtenerProductosBajos(empresa.IdEmpresa);



                return Json(new
                {
                    Data = producto

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

    }
}