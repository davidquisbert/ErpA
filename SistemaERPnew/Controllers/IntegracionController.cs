using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logica;
using Datos;
using Entidad;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using Entidad.Estados;

namespace SistemaERPnew.Controllers
{
    public class IntegracionController : Controller
    {
        // GET: Integracion
        private LCuenta logica = LCuenta.Logica.LCuenta;
        public ActionResult Index()
        {
            try
            {
                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                ViewBag.CuentaDetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(empresa.IdEmpresa);

                return View();
            }
            catch (Exception ex)
            {
                return JavaScript("MostrarMensaje('Ha ocurrido un error.');");
            }
        }
        public ActionResult Registro(int integracionselect, int idCaja, int idCreditoFiscal, int idDebitoFiscal, int idCompras, int idVentas,int idIt,int idItPorPagar)
        {

            try
            {

                Usuario usuario = (Usuario)Session["Usuario"];
                Empresa empresa = (Empresa)Session["Empresa"];
                // Empresa empresa = (Empresa)Session["Empresa"];
                // Entidades = lLogica.ObtenerLista(estado);
                bool inte;
                if (integracionselect == 0)
                {
                    inte = false;
                }
                else
                {
                    inte = true;
                }
                Empresa integracionlist = new Empresa()
                {
                    Integracion = inte,
                    IdCuentaCaja = idCaja,
                    IdCuentaCreditoFiscal = idCreditoFiscal,
                    IdCuentaDebitoFiscal = idDebitoFiscal,
                    IdCuentaCompras = idCompras,
                    IdCuentaVentas = idVentas,
                    IdCuentaIt = idIt,
                    IdCuentaItPorPagar=idItPorPagar

                };
                logica.RegistroIntegracion(integracionlist,empresa.IdEmpresa);

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
        public ActionResult ObtenerIntegracion()
        {
            try
            {


                Empresa empresa = (Empresa)Session["Empresa"];
                var empresaintegracion = LCuenta.Logica.LCuenta.obtenerEmpresa(empresa.IdEmpresa);


                return Json(new
                {

                    idcaja = empresaintegracion.IdCuentaCaja,
                    idcreditofiscal = empresaintegracion.IdCuentaCreditoFiscal,
                    iddebitofiscal = empresaintegracion.IdCuentaDebitoFiscal,
                    idcompra = empresaintegracion.IdCuentaCompras,
                    idventa = empresaintegracion.IdCuentaVentas,
                    idit = empresaintegracion.IdCuentaIt,
                    iditporpagar = empresaintegracion.IdCuentaItPorPagar,
                    integracion=empresaintegracion.Integracion

                }); ;

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