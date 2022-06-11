using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidad;
using Entidad.Estados;
namespace Logica
{
    public class LComprobante : LLogica<Comprobante>
    {
        public List<Comprobante> listarComprobante(int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<Comprobante> comprobante = new List<Comprobante>();


                    var comprobantes = (from x in esquema.Comprobante
                                        where x.IdEmpresa == idempresa orderby x.Serie descending
                                        select x).ToList();


                    foreach (var i in comprobantes)
                    {
                        Comprobante c = new Comprobante();
                        c.DetalleComprobante = i.DetalleComprobante;
                        c.Empresa = i.Empresa;
                        c.Estado = i.Estado;
                        c.Fecha = i.Fecha;
                        c.Glosa = i.Glosa;
                        c.IdComprobante = i.IdComprobante;
                        c.IdEmpresa = i.IdEmpresa;
                        c.IdMoneda = i.IdMoneda;
                        c.IdUsuario = i.IdUsuario;
                        c.Moneda = i.Moneda;
                        c.Serie = i.Serie;
                        c.TipoCambio = i.TipoCambio;
                        c.TipoComprobante = i.TipoComprobante;
                        c.Usuario = i.Usuario;

                        comprobante.Add(c);
                    }


                    return comprobante;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de comprobantes");
                }

            }
        }

        
        public Comprobante ObtenerComprobante(int idempresa, int idcomprobante)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Comprobante c = new Comprobante();


                    var comprobantes = (from x in esquema.Comprobante
                                        where x.IdEmpresa == idempresa
                                        && x.IdComprobante == idcomprobante
                                        select x).FirstOrDefault();


                    if (comprobantes != null)
                    {
                        c.DetalleComprobante = comprobantes.DetalleComprobante;
                        c.Empresa = comprobantes.Empresa;
                        c.Estado = comprobantes.Estado;
                        c.Fecha = comprobantes.Fecha;
                        c.Glosa = comprobantes.Glosa;
                        c.IdComprobante = comprobantes.IdComprobante;
                        c.IdEmpresa = comprobantes.IdEmpresa;
                        c.IdMoneda = comprobantes.IdMoneda;
                        c.IdUsuario = comprobantes.IdUsuario;
                        c.Moneda = comprobantes.Moneda;
                        c.Serie = comprobantes.Serie;
                        c.TipoCambio = comprobantes.TipoCambio;
                        c.TipoComprobante = comprobantes.TipoComprobante;
                        c.Usuario = comprobantes.Usuario;
                    }
                    else
                    {
                        throw new MensageException("Error no se pudo obtener el comprobante");
                    }
                    return c;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se pudo obtener el comprobante");
                }

            }
        }

        public List<EDetalleComprobante> listarDetalleComprobanteXComprobante(int idcomprobante, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    List<EDetalleComprobante> detalleComprobantes = new List<EDetalleComprobante>();


                    var dcomprobantes = (from x in esquema.DetalleComprobante
                                         where x.IdComprobante == idcomprobante
                                         select x).ToList();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    foreach (var i in dcomprobantes)
                    {

                        EDetalleComprobante e = new EDetalleComprobante();

                        e.IdCuenta = i.IdCuenta;
                        e.Cuenta = i.Cuenta.Codigo + " " + i.Cuenta.Nombre;
                        e.Glosa = i.Glosa;

                        if (i.Comprobante.IdMoneda == moneda.IdMonedaPrincipal)
                        {
                            e.Debe = i.MontoDebe;
                            e.Haber = i.MontoHaber;
                        }
                        else
                        {
                            e.Debe = i.MontoDebeAlt;
                            e.Haber = i.MontoHaberAlt;
                        }

                        detalleComprobantes.Add(e);

                    }

                    return detalleComprobantes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
                }

            }
        }

        public List<EDetalleTotal> TotalDetalle(List<EDetalleComprobante> detalleComprobantes)
        {


            try
            {

                List<EDetalleTotal> eDetalleTotals = new List<EDetalleTotal>();
                EDetalleTotal detalleTotal = new EDetalleTotal();
                detalleTotal.TotalDebe = 0;
                detalleTotal.TotalHaber = 0;


                foreach (var i in detalleComprobantes)
                {
                    detalleTotal.TotalDebe = detalleTotal.TotalDebe + i.Debe;
                    detalleTotal.TotalHaber = detalleTotal.TotalHaber + i.Haber;
                }

                eDetalleTotals.Add(detalleTotal);

                return eDetalleTotals;


            }
            catch (Exception ex)
            {
                throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
            }

        }

        

        public List<EComprobante> ObtenerComprobanteReporte(int idempresa, int idcomprobante)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<EComprobante> eComprobantes = new List<EComprobante>();
                    EComprobante c = new EComprobante();


                    var comprobantes = (from x in esquema.Comprobante
                                        where x.IdEmpresa == idempresa
                                        && x.IdComprobante == idcomprobante
                                        select x).FirstOrDefault();


                    if (comprobantes != null)
                    {

                        c.Serie = comprobantes.Serie;
                        c.Glosa = comprobantes.Glosa;
                        c.TipoCambio = comprobantes.TipoCambio;
                        c.Moneda = comprobantes.Moneda.Abreviatura;
                        c.Fecha = comprobantes.Fecha.ToString("dd/MM/yyyy");

                        switch (comprobantes.Estado)
                        {
                            case (int)EstadoComprobante.Abierto:
                                c.Estado = "Abierto";
                                break;
                            case (int)EstadoComprobante.Cerrado:
                                c.Estado = "Cerrado";
                                break;
                            case (int)EstadoComprobante.Anualdo:
                                c.Estado = "Anulado";
                                break;
                        }

                        switch (comprobantes.TipoComprobante)
                        {
                            case (int)TipoComprobante.Apertura:
                                c.TipoComprobante = "Apertura";
                                break;
                            case (int)TipoComprobante.Egreso:
                                c.TipoComprobante = "Egreso";
                                break;
                            case (int)TipoComprobante.Ingreso:
                                c.TipoComprobante = "Ingreso";
                                break;
                            case (int)TipoComprobante.Traspaso:
                                c.TipoComprobante = "Traspaso";
                                break;
                            case (int)TipoComprobante.Ajuste:
                                c.TipoComprobante = "Ajuste";
                                break;


                        }


                    }
                    else
                    {
                        throw new MensageException("Error no se puedo obtener el comprobantes");
                    }



                    eComprobantes.Add(c);
                    return eComprobantes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener el comprobantes");
                }

            }
        }

        public int obtenerSerie(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    int Serie = 1;
                    var comprobantes = (from x in esquema.Comprobante
                                        where x.IdEmpresa == idempresa
                                        orderby x.IdComprobante descending
                                        select x).FirstOrDefault();

                    if (comprobantes != null)
                    {
                        Serie = Serie + comprobantes.Serie;
                    }
                    return Serie;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error No se pudo obtener El numero de serie");
                }

            }
        }
        public Comprobante Registro(Comprobante Entidad, List<EDetalleComprobante> eDetalleComprobante, EDetalleTotal DetalleTotal)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);
                    ValidacionDetalleTotal(DetalleTotal);
                    ValidacionExisteDetalle(DetalleTotal);


                    var gestion = (from x in esquema.Gestion
                                   where Entidad.Fecha >= x.Fechainicio
                                   && Entidad.Fecha <= x.Fechafin
                                   && x.IdEmpresa == Entidad.IdEmpresa
                                   && x.Estado == (int)EstadoGestion.Abierto
                                   select x).FirstOrDefault();

                    if (gestion != null)
                    {

                        var periodo = (from x in esquema.Periodo
                                       where Entidad.Fecha >= x.Fechainicio
                                       && Entidad.Fecha <= x.Fechafin
                                       && x.IdGestion == gestion.IdGestion
                                       && x.Estado == (int)EstadoPeriodo.Abierto
                                       select x).FirstOrDefault();
                        if (periodo != null)
                        {

                            var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(Entidad.IdEmpresa);
                            

                            if (Entidad.TipoComprobante == (int)TipoComprobante.Apertura)
                            {

                                var comprobante = (from x in esquema.Comprobante
                                                   where x.TipoComprobante == (int)TipoComprobante.Apertura
                                                   && x.Estado != (int)EstadoComprobante.Anualdo
                                                   && x.IdEmpresa == Entidad.IdEmpresa
                                                   && x.Fecha >= gestion.Fechainicio
                                                   && x.Fecha <= gestion.Fechafin
                                                   select x).FirstOrDefault();
                                if (comprobante != null)
                                {
                                    throw new MensageException("Ya existe un comprobante de apertura en esta gestion");
                                }
                                else
                                {
                                    esquema.Comprobante.Add(Entidad);
                                    esquema.SaveChanges();
                                    int contador = 0;

                                    foreach (var i in eDetalleComprobante)
                                    {
                                        DetalleComprobante d = new DetalleComprobante();

                                        contador = contador + 1;
                                        d.Numero = contador;
                                        d.Glosa = i.Glosa;
                                        d.IdCuenta = i.IdCuenta;
                                        d.IdComprobante = Entidad.IdComprobante;
                                        d.IdUsuario = Entidad.IdUsuario;

                                        if (moneda.Cambio == null)
                                        {
                                            d.MontoDebe = i.Debe;
                                            d.MontoHaber = i.Haber;
                                            d.MontoDebeAlt = 0;
                                            d.MontoHaberAlt = 0;
                                        }
                                        else
                                        {
                                        if (moneda.IdMonedaPrincipal == Entidad.IdMoneda)
                                        {
                                            d.MontoDebe = i.Debe;
                                            d.MontoHaber = i.Haber;
                                            d.MontoDebeAlt = Math.Round((i.Debe / Entidad.TipoCambio), 2);
                                            d.MontoHaberAlt = Math.Round((i.Haber / Entidad.TipoCambio), 2);

                                        }
                                        else
                                        {
                                            d.MontoDebe = Math.Round((i.Debe * Entidad.TipoCambio), 2);
                                            d.MontoHaber = Math.Round((i.Haber * Entidad.TipoCambio), 2);
                                            d.MontoDebeAlt = i.Debe;
                                            d.MontoHaberAlt = i.Haber;
                                        }
                                        }
                                        esquema.DetalleComprobante.Add(d);
                                        esquema.SaveChanges();


                                    }
                                }

                            }
                            else
                            {

                                esquema.Comprobante.Add(Entidad);
                                esquema.SaveChanges();
                                int contador = 0;

                                foreach (var i in eDetalleComprobante)
                                {
                                    DetalleComprobante d = new DetalleComprobante();

                                    contador = contador + 1;
                                    d.Numero = contador;
                                    d.Glosa = i.Glosa;
                                    d.IdCuenta = i.IdCuenta;
                                    d.IdComprobante = Entidad.IdComprobante;
                                    d.IdUsuario = Entidad.IdUsuario;
                                    if (moneda.Cambio == null)
                                    {
                                        d.MontoDebe = i.Debe;
                                        d.MontoHaber = i.Haber;

                                        d.MontoDebeAlt = 0;
                                        d.MontoHaberAlt = 0;
                                    }
                                    else
                                    {
                                    if (moneda.IdMonedaPrincipal == Entidad.IdMoneda)
                                    {
                                        d.MontoDebe = i.Debe;
                                        d.MontoHaber = i.Haber;

                                        d.MontoDebeAlt = Math.Round((i.Debe / Entidad.TipoCambio), 2);
                                        d.MontoHaberAlt = Math.Round((i.Haber / Entidad.TipoCambio), 2);
                                    }
                                    else
                                    {
                                        d.MontoDebe = Math.Round((i.Debe * Entidad.TipoCambio), 2);
                                        d.MontoHaber = Math.Round((i.Haber * Entidad.TipoCambio), 2);
                                        d.MontoDebeAlt = i.Debe;
                                        d.MontoHaberAlt = i.Haber;
                                    }

                                    }
                                    esquema.DetalleComprobante.Add(d);
                                    esquema.SaveChanges();


                                }

                            }


                        }
                        else
                        {
                            throw new MensageException("La Fecha esta fuera de un periodo activo.");
                        }

                    }
                    else
                    {
                        throw new MensageException("La Fecha Esta fuera de una gestion Activa.");
                    }

                    return Entidad;

                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
        public Comprobante AnularComprobante(int idcomprobante)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdComprobante == idcomprobante
                                       select x).FirstOrDefault();
                    var notas= (from x in esquema.Nota
                                where x.IdComprobante == idcomprobante
                                select x).FirstOrDefault();

                    if (notas != null)
                    {
                        throw new MensageException("No se puede anular el comprobante porque pertenece a una nota");
                    }
                    else
                    {
                        if (comprobante != null)
                        {
                            comprobante.Estado = (int)EstadoComprobante.Anualdo;
                            esquema.SaveChanges();
                        }
                        else
                        {
                            throw new MensageException("No se puedo obtener el comprobante");
                        }
                    }
                    

                    return comprobante;
                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
            }

        }

        
        public void EliminarDetalleComprobante(List<EDetalleComprobante> detalleComprobantes, ERPBDEntities esquema, int idcomprobante)
        {
            try
            {
                List<int> idcuenta = new List<int>();

                foreach (var i in detalleComprobantes)
                {
                    idcuenta.Add(i.IdDetalle);
                }

                var dcomprobante = (from x in esquema.DetalleComprobante
                                    where x.IdComprobante == idcomprobante
                                    && !idcuenta.Contains(x.IdCuenta)
                                    select x
                                    ).ToList();

                foreach (var i in dcomprobante)
                {

                    esquema.DetalleComprobante.Remove(i);
                    esquema.SaveChanges();

                }

            }
            catch
            {
                throw new MensageException("Error al modificar el detalle comprobante");
            }
        }


       
        public List<ERDatosBasicoCuenta> ReporteDatosBasicoComprobante(string usuario, string empresa)
        {
            try
            {


                List<ERDatosBasicoCuenta> basicos = new List<ERDatosBasicoCuenta>();
                ERDatosBasicoCuenta eRDatosBasico = new ERDatosBasicoCuenta();
                eRDatosBasico.Usuario = usuario;

                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }
      
        public List<ERDatosBasicoSumasySaldos> ReporteDatosBasicoSumasySaldos(string usuario, string empresa,int idmoneda,int idgestion)
        {
            try
            {
                List<ERDatosBasicoSumasySaldos> basicos = new List<ERDatosBasicoSumasySaldos>();
                ERDatosBasicoSumasySaldos eRDatosBasico = new ERDatosBasicoSumasySaldos();
                var moneda = LMoneda.Logica.LMoneda.obtenerMoneda(idmoneda);
                var gestion = LGestion.Logica.LGestion.obtenerGestion(idgestion);
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreGestion = gestion.Nombre;
                eRDatosBasico.Moneda = moneda.Nombre;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }
        
        public List<ERDatosBasicoBalanceInicial> ReporteDatosBasicoBalanceInicial(string usuario, string empresa, int idmoneda, int idgestion)
        {
            try
            {
                
                List<ERDatosBasicoBalanceInicial> basicos = new List<ERDatosBasicoBalanceInicial>();
                ERDatosBasicoBalanceInicial eRDatosBasicoBalance = new ERDatosBasicoBalanceInicial();

                var moneda = LMoneda.Logica.LMoneda.obtenerMoneda(idmoneda);
                var gestion = LGestion.Logica.LGestion.obtenerGestion(idgestion);


                eRDatosBasicoBalance.Usuario = usuario;
                eRDatosBasicoBalance.NombreEmpresa = empresa;
                eRDatosBasicoBalance.NombreGestion = gestion.Nombre;
                eRDatosBasicoBalance.FechaInicioGestion = gestion.Fechainicio.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaFinGestion = gestion.Fechafin.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.Moneda = moneda.Nombre;

                basicos.Add(eRDatosBasicoBalance);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error logica.");
            }
        }

        public List<ERDatosBasicoComprobante> ReporteDatosBasico(string usuario, string empresa, int idmoneda, int idgestion, int idperiodo)
        {
            try
            {
                List<ERDatosBasicoComprobante> basicos = new List<ERDatosBasicoComprobante>();
                ERDatosBasicoComprobante eRDatosBasico = new ERDatosBasicoComprobante();

                var moneda = LMoneda.Logica.LMoneda.obtenerMoneda(idmoneda);
                var gestion = LGestion.Logica.LGestion.obtenerGestion(idgestion);

                var periodo = new Periodo();

                if (idperiodo == 0)
                {
                    eRDatosBasico.Usuario = usuario;
                    eRDatosBasico.NombreGestion = gestion.Nombre;
                    eRDatosBasico.NombrePeriodo = "Todos";
                    eRDatosBasico.NombreEmpresa = empresa;
                    eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                    eRDatosBasico.Moneda = moneda.Abreviatura;

                }
                else
                {
                    periodo = LPeriodo.Logica.LPeriodo.obtenerPeriodo(idperiodo);
                    eRDatosBasico.Usuario = usuario;
                    eRDatosBasico.NombreGestion = gestion.Nombre;
                    eRDatosBasico.NombrePeriodo = periodo.Nombre;
                    eRDatosBasico.NombreEmpresa = empresa;
                    eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                    eRDatosBasico.Moneda = moneda.Abreviatura;
                }


                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }
        public void Validacion(Comprobante Entidad)
        {

            if (Entidad.TipoCambio < 0)
            {
                throw new MensageException("El cambio debe ser mayor a 0");
            }
            if (string.IsNullOrEmpty(Entidad.Glosa))
            {
                throw new MensageException("Ingrese una glosa");
            }

        }

        public void ValidacionDetalleTotal(EDetalleTotal Entidad)
        {
            if (Entidad.TotalDebe != Entidad.TotalHaber)
            {
                throw new MensageException("Los Totales No Son Iguales");
            }
        }
        public void ValidacionExisteDetalle(EDetalleTotal Entidad)
        {
            if (Entidad.TotalDebe==0 && Entidad.TotalHaber == 0)
            {
                throw new MensageException("Deben haber al menos 2 detalles registrados");
            }
            
        }
        //BALANCE INICIAL
        public List<ECabeceraActivo> cabeceraactivos(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraActivo> cabeceraActivos = new List<ECabeceraActivo>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    
                    var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                           where x.IdEmpresa == idempresa
                                           && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                           && x.Estado != (int)EstadoComprobante.Anualdo
                                           && x.TipoComprobante == (int)TipoComprobante.Apertura
                                           select x).ToList();

                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.Cuenta
                                        where x.IdEmpresa == idempresa
                                        && x.Nivel == 1 && x.Codigo == codigoAuxac
                                        select x).FirstOrDefault();
                    foreach (var i in comprobante)
                        {
                        ECabeceraActivo d = new ECabeceraActivo();
                        d.Codigoactivo = cuentaabueloactivo.Codigo;
                        d.Cuentaactivo = cuentaabueloactivo.Nombre;
                        
                        
                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                            {
                            
                            if (j.MontoDebe > 0)
                            {

                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalactivo = d.Totalactivo + j.MontoDebe;
                                    
                                }
                                else
                                {
                                    d.Totalactivo = d.Totalactivo + j.MontoDebeAlt;
                                }
                            }
                            }
                        cabeceraActivos.Add(d);

                    }

                    return cabeceraActivos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance Inicial");
                }

            }
        }
        public List<ECabeceraPasivoPatrimonio> cabecerapasivopatrimonios(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraPasivoPatrimonio> cabeceraPasivoPatrimonios = new List<ECabeceraPasivoPatrimonio>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       && x.TipoComprobante == (int)TipoComprobante.Apertura
                                       select x).ToList();

                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxpas
                                              select x).FirstOrDefault();
                    var cuentaabuelopatrimonio = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxpat
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraPasivoPatrimonio d = new ECabeceraPasivoPatrimonio();
                        d.Codigopasivo = cuentaabuelopasivo.Codigo;
                        d.Cuentapasivo = cuentaabuelopasivo.Nombre;
                        d.Codigopatrimonio = cuentaabuelopatrimonio.Codigo;
                        d.Cuentapatrimonio = cuentaabuelopatrimonio.Nombre;

                        
                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {
                           
                            if (j.MontoHaber > 0)
                            {
                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaber;

                                }
                                else
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaberAlt;
                                }
                            }
                            

                        }
                        cabeceraPasivoPatrimonios.Add(d);
                    }

                    return cabeceraPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance Inicial");
                }

            }
        }
        
        public List<EBalanceInicialActivo> reporteBalanceInicialActivo(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceInicialActivo> balanceInicialActivos = new List<EBalanceInicialActivo>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa= (from x in esquema.Empresa
                                  where x.IdEmpresa == idempresa
                                  select x).FirstOrDefault();

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       && x.TipoComprobante == (int)TipoComprobante.Apertura
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivos = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxac
                                              select x).ToList();

                     if (empresa.Niveles == 3)
                     {
                           foreach(var l in cuentaabueloactivos)
                           {
                                    var cuentas = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 2 && x.IdCuentaPadre==l.IdCuenta
                                                    select x).ToList();
                                    foreach(var z in cuentas)
                                    {

                                            foreach (var i in comprobante)
                                            {
                                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                                 where x.IdComprobante == i.IdComprobante
                                                                                select x).ToList();
                                                  foreach (var j in detallecomprobante)
                                                  {
                                                             EBalanceInicialActivo d = new EBalanceInicialActivo();
                                                             if (z.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                             {
                                                                    d.CuentaCabecera1 = z.Nombre;
                                                                    d.CodigoCabecera1 = z.Codigo;
                                                                    d.IdCabecera1 = z.IdCuenta;
                                                                   
                                                                if (j.MontoDebe > 0)
                                                                {
                                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                                        d.Cuenta = j.Cuenta.Nombre;
                                
                                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                                        {
                                                                             d.Debe = j.MontoDebe;
                                                                        }
                                                                        else
                                                                        {
                                                                             d.Debe = j.MontoDebeAlt;
                                                                        }
                                                                        d.TotalCabecera1=d.TotalCabecera1+d.Debe;
                                                                        totalgeneral=totalgeneral+d.TotalCabecera1;
                                                                        balanceInicialActivos.Add(d);
                                                                }
                                                                else if(j.MontoHaber > 0)
                                                                {
                                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                                        d.Cuenta = j.Cuenta.Nombre;

                                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                                        {
                                                                             d.Debe = j.MontoHaber*(-1);
                                                                        }
                                                                        else
                                                                        {
                                                                            d.Debe = j.MontoHaberAlt*(-1);
                                                                        }
                                                                        d.TotalCabecera1=d.TotalCabecera1+d.Debe;
                                                                        totalgeneral=totalgeneral+d.TotalCabecera1;
                                                                        balanceInicialActivos.Add(d);
                                                                }

                                                             }

                                                  }

                                            }
                                    }
                           }
                     }else if (empresa.Niveles == 4)
                     {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();

                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach(var q in cuentas2)
                                {

                                foreach (var i in comprobante)
                                {
                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();
                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceInicialActivo d = new EBalanceInicialActivo();
                                        if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {   d.CuentaCabecera5=z.Nombre;
                                            d.CodigoCabecera5 = z.Codigo;
                                            d.IdCabecera5 = z.IdCuenta;
                                            d.CuentaCabecera1 = q.Nombre;
                                            d.CodigoCabecera1 = q.Codigo;
                                            d.IdCabecera1 = q.IdCuenta;

                                            if (j.MontoDebe > 0)
                                            {
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Debe = j.MontoDebe;
                                                }
                                                else
                                                {
                                                    d.Debe = j.MontoDebeAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialActivos.Add(d);
                                            }
                                            else if (j.MontoHaber > 0)
                                            {
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Debe = j.MontoHaber * (-1);
                                                }
                                                else
                                                {
                                                    d.Debe = j.MontoHaberAlt * (-1);
                                                }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialActivos.Add(d);
                                                }

                                        }

                                    }

                                }


                                }
                            }
                        }
                     }
                     else if (empresa.Niveles == 5)
                     {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {
                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                 
                                    foreach (var w in cuentas3)
                                    {

                                    foreach (var i in comprobante)
                                    {
                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();
                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceInicialActivo d = new EBalanceInicialActivo();
                                            if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                    d.IdCabecera5 = z.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;

                                                if (j.MontoDebe > 0)
                                                {
                                                    d.CodigoCuenta =j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Debe = j.MontoDebe;
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.MontoDebeAlt;
                                                    }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialActivos.Add(d);
                                                }
                                                else if (j.MontoHaber > 0)
                                                {
                                                    d.CodigoCuenta =  j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Debe = j.MontoHaber * (-1);
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.MontoHaberAlt * (-1);
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialActivos.Add(d);
                                                }

                                            }

                                        }

                                    }


                                    }

                                }
                            }
                        }
                     }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach(var e in cuentas4)
                                        {

                                        foreach (var i in comprobante)
                                        {
                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();
                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceInicialActivo d = new EBalanceInicialActivo();
                                                if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = e.Nombre;
                                                    d.CodigoCabecera1 = e.Codigo;
                                                    d.IdCabecera1 = e.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera5 = z.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera3 = w.Nombre;
                                                    d.CodigoCabecera3 = w.Codigo;
                                                    d.IdCabecera3 = w.IdCuenta;


                                                        if (j.MontoDebe > 0)
                                                    {
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Debe = j.MontoDebe;
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.MontoDebeAlt;
                                                        }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;


                                                            balanceInicialActivos.Add(d);
                                                    }
                                                    else if (j.MontoHaber > 0)
                                                    {
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Debe = j.MontoHaber * (-1);
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.MontoHaberAlt * (-1);
                                                        }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;

                                                            balanceInicialActivos.Add(d);
                                                    }

                                                }

                                            }

                                        }


                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {
                                            var cuentas5 = (from x in esquema.Cuenta
                                                            where x.IdEmpresa == idempresa
                                                            && x.Nivel == 6 && x.IdCuentaPadre == e.IdCuenta
                                                            select x).ToList();
                                            foreach(var r in cuentas5)
                                            {
                                            foreach (var i in comprobante)
                                            {
                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();
                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceInicialActivo d = new EBalanceInicialActivo();
                                                    if (r.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = r.Nombre;
                                                        d.CodigoCabecera1 = r.Codigo;
                                                        d.IdCabecera1 = r.IdCuenta;
                                                        d.CuentaCabecera2 = e.Nombre;
                                                        d.CodigoCabecera2 = e.Codigo;
                                                        d.IdCabecera2 = e.IdCuenta;
                                                        d.CuentaCabecera3 = w.Nombre;
                                                        d.CodigoCabecera3 = w.Codigo;
                                                        d.IdCabecera3 = w.IdCuenta;
                                                        d.CuentaCabecera4 = q.Nombre;
                                                        d.CodigoCabecera4 = q.Codigo;
                                                        d.IdCabecera4 = q.IdCuenta;
                                                        d.CuentaCabecera5 = z.Nombre;
                                                        d.CodigoCabecera5 = z.Codigo;
                                                        d.IdCabecera5 = z.IdCuenta;

                                                            if (j.MontoDebe > 0)
                                                        {
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Debe = j.MontoDebe;
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.MontoDebeAlt;
                                                            }
                                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                                d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                                d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                                
                                                            balanceInicialActivos.Add(d);
                                                        }
                                                        else if (j.MontoHaber > 0)
                                                        {
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Debe = j.MontoHaber * (-1);
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.MontoHaberAlt * (-1);
                                                            }
                                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                                d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                                d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                                balanceInicialActivos.Add(d);
                                                        }

                                                    }

                                                }

                                            }


                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }
                    return balanceInicialActivos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance Inicial");
                }

            }
        }
        public List<EBalanceInicialPasivoPatrimonio> reporteBalanceInicialPasivoPatrimonio(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceInicialPasivoPatrimonio> balanceInicialPasivoPatrimonios = new List<EBalanceInicialPasivoPatrimonio>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       && x.TipoComprobante == (int)TipoComprobante.Apertura
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.Cuenta
                                               where x.IdEmpresa == idempresa
                                               && x.Nivel == 1 && x.Codigo == codigoAuxpas || x.Codigo == codigoAuxpat
                                                        select x).ToList();
                    if (empresa.Niveles == 3)
                    {
                        foreach(var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach(var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {

                                     var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                where x.IdComprobante == i.IdComprobante
                                                                select x).ToList();

                                     foreach (var j in detallecomprobante)
                                     {
                                            EBalanceInicialPasivoPatrimonio d = new EBalanceInicialPasivoPatrimonio();
                                            if (z.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = z.Nombre;
                                                d.CodigoCabecera1 = z.Codigo;
                                                d.IdCabecera1 = z.IdCuenta;
                                                
                                            if (j.MontoHaber > 0)
                                                {
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;
                              
                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 +d.Haber;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                                balanceInicialPasivoPatrimonios.Add(d);
                                                }else if(j.MontoDebe > 0)
                                                {
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoDebe*(-1);
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoDebeAlt*(-1);
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1+ d.Haber;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                                balanceInicialPasivoPatrimonios.Add(d);
                                                }
                                            }
                                     }
                                }
                            }
                        }
                    }else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                               where x.IdEmpresa == idempresa
                                               && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                               select x).ToList();
                                foreach(var q in cuentas1)
                                {

                                foreach (var i in comprobante)
                                {

                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();

                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceInicialPasivoPatrimonio d = new EBalanceInicialPasivoPatrimonio();
                                        if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = q.Nombre;
                                            d.CodigoCabecera1 = q.Codigo;
                                            d.IdCabecera1 = q.IdCuenta;
                                            d.CuentaCabecera5 = z.Nombre;
                                            d.CodigoCabecera5 = z.Codigo;
                                            d.IdCabecera5 = z.IdCuenta;

                                                if (j.MontoHaber > 0)
                                            {
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.MontoHaber;
                                                }
                                                else
                                                {
                                                    d.Haber = j.MontoHaberAlt;
                                                }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialPasivoPatrimonios.Add(d);
                                                }
                                            else if (j.MontoDebe > 0)
                                            {
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.MontoDebe * (-1);
                                                }
                                                else
                                                {
                                                    d.Haber = j.MontoDebeAlt * (-1);
                                                }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialPasivoPatrimonios.Add(d);
                                                }
                                        }
                                    }
                                }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach(var w in cuentas2)
                                    {
                                    foreach (var i in comprobante)
                                    {

                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();

                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceInicialPasivoPatrimonio d = new EBalanceInicialPasivoPatrimonio();
                                            if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5= z.Codigo;
                                                    d.IdCabecera5 = z.IdCuenta;

                                                    if (j.MontoHaber > 0)
                                                {
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoHaberAlt;
                                                    }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialPasivoPatrimonios.Add(d);
                                                    }
                                                else if (j.MontoDebe > 0)
                                                {
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoDebe * (-1);
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoDebeAlt * (-1);
                                                    }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialPasivoPatrimonios.Add(d);
                                                    }
                                            }
                                        }
                                    }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach(var e in cuentas3)
                                        {
                                        foreach (var i in comprobante)
                                        {

                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();

                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceInicialPasivoPatrimonio d = new EBalanceInicialPasivoPatrimonio();
                                                if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = e.Nombre;
                                                    d.CodigoCabecera1 = e.Codigo;
                                                    d.IdCabecera1 = e.IdCuenta;
                                                    d.CuentaCabecera3 = w.Nombre;
                                                    d.CodigoCabecera3 = w.Codigo;
                                                    d.IdCabecera3 = w.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera5 = z.IdCuenta;

                                                        if (j.MontoHaber > 0)
                                                    {
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.MontoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.MontoHaberAlt;
                                                        }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            balanceInicialPasivoPatrimonios.Add(d);
                                                        }
                                                    else if (j.MontoDebe > 0)
                                                    {
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.MontoDebe * (-1);
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.MontoDebeAlt * (-1);
                                                        }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            balanceInicialPasivoPatrimonios.Add(d);
                                                        }
                                                }
                                            }
                                        }


                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            var cuentas4 = (from x in esquema.Cuenta
                                                            where x.IdEmpresa == idempresa
                                                            && x.Nivel == 6 && x.IdCuentaPadre == e.IdCuenta
                                                            select x).ToList();
                                            foreach(var r in cuentas4)
                                            {
                                            foreach (var i in comprobante)
                                            {

                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();

                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceInicialPasivoPatrimonio d = new EBalanceInicialPasivoPatrimonio();
                                                    if (r.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = r.Nombre;
                                                        d.CodigoCabecera1 = r.Codigo;
                                                        d.IdCabecera1 = r.IdCuenta;
                                                        d.CuentaCabecera2 = e.Nombre;
                                                        d.CodigoCabecera2 = e.Codigo;
                                                        d.IdCabecera2 = e.IdCuenta;
                                                        d.CuentaCabecera3 = w.Nombre;
                                                        d.CodigoCabecera3 = w.Codigo;
                                                        d.IdCabecera3 = w.IdCuenta;
                                                        d.CuentaCabecera4= q.Nombre;
                                                        d.CodigoCabecera4 = q.Codigo;
                                                        d.IdCabecera4 = q.IdCuenta;
                                                        d.CuentaCabecera5 = z.Nombre;
                                                        d.CodigoCabecera5 = z.Codigo;
                                                        d.IdCabecera5 = z.IdCuenta;

                                                            if (j.MontoHaber > 0)
                                                        {
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Haber = j.MontoHaber;
                                                            }
                                                            else
                                                            {
                                                                d.Haber = j.MontoHaberAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            balanceInicialPasivoPatrimonios.Add(d);
                                                        }
                                                        else if (j.MontoDebe > 0)
                                                        {
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Haber = j.MontoDebe * (-1);
                                                            }
                                                            else
                                                            {
                                                                d.Haber = j.MontoDebeAlt * (-1);
                                                            }
                                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                                d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                                d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                                balanceInicialPasivoPatrimonios.Add(d);
                                                            }
                                                    }
                                                }
                                            }


                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return balanceInicialPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance Inicial");
                }

            }
        }
        //LIBRO DIARIO
        public List<ELibroDiario> reporteLibroDiario(int idgestion, int idperiodo, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ELibroDiario> libroDiarios = new List<ELibroDiario>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    if (idperiodo == 0)
                    {
                        var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();

                        var comprobante = (from x in esquema.Comprobante
                                           where x.IdEmpresa == idempresa
                                           && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                           && x.Estado != (int)EstadoComprobante.Anualdo
                                           orderby x.Fecha ascending
                                           select x).ToList();



                        foreach (var i in comprobante)
                        {
                            ELibroDiario e = new ELibroDiario();
                            e.Fecha = i.Fecha.ToString("dd/MM/yyyy");
                            e.Cuenta = i.Glosa;
                            libroDiarios.Add(e);

                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                      where x.IdComprobante == i.IdComprobante
                                                      orderby x.MontoDebe descending
                                                      select x).ToList();

                            foreach (var j in detallecomprobante)
                            {

                                ELibroDiario d = new ELibroDiario();
                                if (j.MontoDebe > 0)
                                {
                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                    d.Cuenta = j.Cuenta.Codigo +" - "+ j.Cuenta.Nombre;
                                }
                                else
                                {
                                    d.CodigoCuenta = "      " + j.Cuenta.Codigo;
                                    d.Cuenta = "      " + j.Cuenta.Codigo +" - "+ j.Cuenta.Nombre;
                                }

                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Debe = j.MontoDebe;
                                    d.Haber = j.MontoHaber;
                                }
                                else
                                {
                                    d.Debe = j.MontoDebeAlt;
                                    d.Haber = j.MontoHaberAlt;
                                }

                                libroDiarios.Add(d);

                            }

                        }

                    }
                    else
                    {

                        var periodo = (from x in esquema.Periodo
                                       where x.IdPeriodo == idperiodo
                                       select x).FirstOrDefault();

                        var comprobante = (from x in esquema.Comprobante
                                           where x.IdEmpresa == idempresa
                                           && x.Fecha >= periodo.Fechainicio && x.Fecha <= periodo.Fechafin
                                           && x.Estado != (int)EstadoComprobante.Anualdo
                                           orderby x.Fecha ascending
                                           select x).ToList();



                        foreach (var i in comprobante)
                        {
                            ELibroDiario e = new ELibroDiario();
                            e.Fecha = i.Fecha.ToString("dd/MM/yyyy");
                            e.Cuenta = i.Glosa;
                            libroDiarios.Add(e);

                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                      where x.IdComprobante == i.IdComprobante
                                                      orderby x.MontoDebe descending
                                                      select x).ToList();

                            foreach (var j in detallecomprobante)
                            {

                                ELibroDiario d = new ELibroDiario();
                                if (j.MontoDebe > 0)
                                {
                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                    d.Cuenta = j.Cuenta.Codigo + " - " + j.Cuenta.Nombre;
                                }
                                else
                                {
                                    d.CodigoCuenta = "      " + j.Cuenta.Codigo;
                                    d.Cuenta = "      " + j.Cuenta.Codigo + " - " + j.Cuenta.Nombre;
                                }


                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Debe = j.MontoDebe;
                                    d.Haber = j.MontoHaber;
                                }
                                else
                                {
                                    d.Debe = j.MontoDebeAlt;
                                    d.Haber = j.MontoHaberAlt;
                                }

                                libroDiarios.Add(d);

                            }

                        }


                    }

                    return libroDiarios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener la lista de detalle comprobantes");
                }

            }
        }
        public List<EDetalleTotal> TotalLibroDiario(List<ELibroDiario> detalleComprobantes)
        {


            try
            {

                List<EDetalleTotal> eDetalleTotals = new List<EDetalleTotal>();
                EDetalleTotal detalleTotal = new EDetalleTotal();
                detalleTotal.TotalDebe = 0;
                detalleTotal.TotalHaber = 0;


                foreach (var i in detalleComprobantes)
                {
                    detalleTotal.TotalDebe = detalleTotal.TotalDebe + i.Debe;
                    detalleTotal.TotalHaber = detalleTotal.TotalHaber + i.Haber;
                }

                eDetalleTotals.Add(detalleTotal);

                return eDetalleTotals;


            }
            catch (Exception ex)
            {
                throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
            }

        }
        //LIBRO MAYOR
        public List<ELibroMayoCabezera> reporteLibroMayorCabezera(int idgestion, int idperiodo, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ELibroMayoCabezera> libroMayoCabezeras = new List<ELibroMayoCabezera>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    if (idperiodo == 0)
                    {
                        var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();

                        var cuentadetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(idempresa);


                        foreach (var i in cuentadetalle)
                        {


                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                      where x.Comprobante.Fecha >= gestion.Fechainicio
                                                      && x.Comprobante.Fecha <= gestion.Fechafin
                                                      && x.Comprobante.Estado != (int)EstadoComprobante.Anualdo
                                                      && x.IdCuenta == i.IdCuenta
                                                      select x).FirstOrDefault();


                            if (detallecomprobante != null)
                            {
                                ELibroMayoCabezera m = new ELibroMayoCabezera();
                                m.IdCuenta = i.IdCuenta;
                                m.Cuenta = i.Codigo + " " + i.Nombre;
                                libroMayoCabezeras.Add(m);
                            }


                        }

                    }
                    else
                    {

                        var periodo = (from x in esquema.Periodo
                                       where x.IdPeriodo == idperiodo
                                       select x).FirstOrDefault();

                        var cuentadetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(idempresa);


                        foreach (var i in cuentadetalle)
                        {


                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                      where x.Comprobante.Fecha >= periodo.Fechainicio
                                                      && x.Comprobante.Fecha <= periodo.Fechafin
                                                      && x.IdCuenta == i.IdCuenta
                                                      select x).FirstOrDefault();


                            if (detallecomprobante != null)
                            {
                                ELibroMayoCabezera m = new ELibroMayoCabezera();
                                m.IdCuenta = i.IdCuenta;
                                m.Cuenta = i.Codigo + " " + i.Nombre;
                                libroMayoCabezeras.Add(m);
                            }

                        }
                    }

                    return libroMayoCabezeras;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
                }

            }
        }
        public List<ELibroMayor> reporteLibroMayor(int idcuenta, int idgestion, int idperiodo, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ELibroMayor> libroMayor = new List<ELibroMayor>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    if (idperiodo == 0)
                    {
                        var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();

                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.Comprobante.Fecha >= gestion.Fechainicio
                                                  && x.Comprobante.Fecha <= gestion.Fechafin
                                                  && x.Comprobante.Estado != (int)EstadoComprobante.Anualdo
                                                  && x.IdCuenta == idcuenta
                                                  select x).ToList();

                        double saldo = 0;

                        foreach (var i in detallecomprobante)
                        {
                            ELibroMayor m = new ELibroMayor();
                            m.IdCuenta = i.IdCuenta;
                            m.Cuenta = i.Cuenta.Codigo + " " + i.Cuenta.Nombre;
                            m.Fecha = i.Comprobante.Fecha.ToString("dd/MM/yyyy");
                            if (moneda.IdMonedaPrincipal == idmoneda)
                            {
                                m.NroComprobante = i.Comprobante.Serie;
                                switch (i.Comprobante.TipoComprobante)
                                {
                                    case (int)TipoComprobante.Apertura:
                                        m.Tipo = "Apertura";
                                        break;
                                    case (int)TipoComprobante.Egreso:
                                        m.Tipo = "Egreso";
                                        break;
                                    case (int)TipoComprobante.Ingreso:
                                        m.Tipo = "Ingreso";
                                        break;
                                    case (int)TipoComprobante.Traspaso:
                                        m.Tipo = "Traspaso";
                                        break;
                                    case (int)TipoComprobante.Ajuste:
                                        m.Tipo = "Ajuste";
                                        break;
                                }
                                m.Glosa = i.Comprobante.Glosa;
                                m.Debe = i.MontoDebe;
                                m.Haber = i.MontoHaber;

                                if (m.Debe > 0)
                                {
                                    saldo = saldo + m.Debe;
                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo;
                                    }

                                }
                                else
                                {
                                    saldo = saldo - m.Haber;

                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo ;
                                    }



                                }

                            }
                            else
                            {
                                m.NroComprobante = i.Comprobante.Serie;
                                switch (i.Comprobante.TipoComprobante)
                                {
                                    case (int)TipoComprobante.Apertura:
                                        m.Tipo = "Apertura";
                                        break;
                                    case (int)TipoComprobante.Egreso:
                                        m.Tipo = "Egreso";
                                        break;
                                    case (int)TipoComprobante.Ingreso:
                                        m.Tipo = "Ingreso";
                                        break;
                                    case (int)TipoComprobante.Traspaso:
                                        m.Tipo = "Traspaso";
                                        break;
                                    case (int)TipoComprobante.Ajuste:
                                        m.Tipo = "Ajuste";
                                        break;


                                }
                                m.Glosa = i.Comprobante.Glosa;
                                m.Debe = i.MontoDebeAlt;
                                m.Haber = i.MontoHaberAlt;

                                if (m.Debe > 0)
                                {
                                    saldo = Math.Round((saldo + m.Debe), 2);
                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo ;
                                    }
                                    //m.Saldo = saldo;

                                }
                                else
                                {
                                    saldo = Math.Round((saldo - m.Haber), 2);

                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo ;
                                    }



                                }


                            }

                            libroMayor.Add(m);

                        }

                    }
                    else
                    {

                        var periodo = (from x in esquema.Periodo
                                       where x.IdPeriodo == idperiodo
                                       select x).FirstOrDefault();

                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.Comprobante.Fecha >= periodo.Fechainicio
                                                  && x.Comprobante.Fecha <= periodo.Fechafin
                                                  && x.Comprobante.Estado != (int)EstadoComprobante.Anualdo
                                                  && x.IdCuenta == idcuenta
                                                  select x).ToList();

                        double saldo = 0;

                        foreach (var i in detallecomprobante)
                        {
                            ELibroMayor m = new ELibroMayor();
                            m.IdCuenta = i.IdCuenta;
                            m.Cuenta = i.Cuenta.Codigo +" "+ i.Cuenta.Nombre;
                            m.Glosa = i.Comprobante.Glosa;
                            m.Fecha = i.Comprobante.Fecha.ToString("dd/MM/yyyy");
                            if (moneda.IdMonedaPrincipal == idmoneda)
                            {

                                m.NroComprobante = i.Comprobante.Serie;
                                switch (i.Comprobante.TipoComprobante)
                                {
                                    case (int)TipoComprobante.Apertura:
                                        m.Tipo = "Apertura";
                                        break;
                                    case (int)TipoComprobante.Egreso:
                                        m.Tipo = "Egreso";
                                        break;
                                    case (int)TipoComprobante.Ingreso:
                                        m.Tipo = "Ingreso";
                                        break;
                                    case (int)TipoComprobante.Traspaso:
                                        m.Tipo = "Traspaso";
                                        break;
                                    case (int)TipoComprobante.Ajuste:
                                        m.Tipo = "Ajuste";
                                        break;
                                }

                                m.Debe = i.MontoDebe;
                                m.Haber = i.MontoHaber;

                                if (m.Debe > 0)
                                {
                                    saldo = saldo + m.Debe;
                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo;
                                    }
                                    //m.Saldo = saldo;

                                }
                                else
                                {
                                    saldo = saldo - m.Haber;

                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo ;
                                    }
                                }

                            }
                            else
                            {
                                m.NroComprobante = i.Comprobante.Serie;
                                switch (i.Comprobante.TipoComprobante)
                                {
                                    case (int)TipoComprobante.Apertura:
                                        m.Tipo = "Apertura";
                                        break;
                                    case (int)TipoComprobante.Egreso:
                                        m.Tipo = "Egreso";
                                        break;
                                    case (int)TipoComprobante.Ingreso:
                                        m.Tipo = "Ingreso";
                                        break;
                                    case (int)TipoComprobante.Traspaso:
                                        m.Tipo = "Traspaso";
                                        break;
                                    case (int)TipoComprobante.Ajuste:
                                        m.Tipo = "Ajuste";
                                        break;


                                }

                                m.Debe = i.MontoDebeAlt;
                                m.Haber = i.MontoHaberAlt;

                                if (m.Debe > 0)
                                {
                                    saldo = Math.Round((saldo + m.Debe), 2);
                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo ;
                                    }
                                    

                                }
                                else
                                {
                                    saldo = Math.Round((saldo - m.Haber), 2);

                                    if (saldo >= 0)
                                    {
                                        m.Saldo = saldo;
                                    }
                                    else
                                    {
                                        m.Saldo = saldo;
                                    }
                                }
                            }
                            libroMayor.Add(m);
                        }
                    }

                    return libroMayor;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
                }

            }
        }
        //SUMAS Y SALDOS
        public List<ELibroMayoCabezera> reporteSumasySaldosCabezera(int idgestion, int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<ELibroMayoCabezera> libroMayoCabezeras = new List<ELibroMayoCabezera>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                        var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();

                        var cuentadetalle = LCuenta.Logica.LCuenta.listarCuentaDetalle(idempresa);


                        foreach (var i in cuentadetalle)
                        {


                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                      where x.Comprobante.Fecha >= gestion.Fechainicio
                                                      && x.Comprobante.Fecha <= gestion.Fechafin
                                                      && x.Comprobante.Estado != (int)EstadoComprobante.Anualdo
                                                      && x.IdCuenta == i.IdCuenta
                                                      select x).FirstOrDefault();

                            if (detallecomprobante != null)
                            {
                                ELibroMayoCabezera m = new ELibroMayoCabezera();
                                m.IdCuenta = i.IdCuenta;
                                m.Cuenta = i.Codigo + " " + i.Nombre;
                                libroMayoCabezeras.Add(m);
                            }


                        }

                    

                    return libroMayoCabezeras;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
                }

            }
        }

        public List<ESumasySaldos> reporteSumasySaldos(int idcuenta, int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ESumasySaldos> sumasysaldos = new List<ESumasySaldos>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                   
                        var gestion = (from x in esquema.Gestion
                                       where x.IdGestion == idgestion
                                       select x).FirstOrDefault();

                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.Comprobante.Fecha >= gestion.Fechainicio
                                                  && x.Comprobante.Fecha <= gestion.Fechafin
                                                  && x.Comprobante.Estado != (int)EstadoComprobante.Anualdo
                                                  && x.IdCuenta == idcuenta
                                                  select x).ToList();

                        double sumadebe = 0;
                        double sumahaber = 0;
                        double saldo = 0;
                    ESumasySaldos m = new ESumasySaldos();
                    foreach (var i in detallecomprobante)
                     {
                            m.Cuenta = i.Cuenta.Codigo + " " + i.Cuenta.Nombre;
                            if (moneda.IdMonedaPrincipal == idmoneda)
                            {
                                sumadebe = sumadebe + i.MontoDebe;
                                sumahaber = sumahaber + i.MontoHaber;
                            }
                            else
                            {
                                sumadebe = sumadebe + i.MontoDebeAlt;
                                sumahaber = sumahaber + i.MontoHaberAlt;
                            }
                            saldo = sumadebe - sumahaber;
                            
                     }
                             m.SumaDebe = sumadebe;
                             m.SumaHaber = sumahaber;
                                    if (saldo > 0)
                                    {
                                         m.SaldoDebe = saldo;
                                         m.SaldoHaber = 0;
                                    }
                                    else if (saldo < 0)
                                    {
                                         m.SaldoDebe = 0;
                                         m.SaldoHaber = saldo*-1;
                                
                                    }

                    sumasysaldos.Add(m); ;
                    return sumasysaldos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle comprobantes");
                }

            }
        }
        //ESTADO DE RESULTADOS
        public List<ECabeceraEstadoResultadosIngreso> cabeceraestadoresultadosingreso(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraEstadoResultadosIngreso> cabeceraIngresos = new List<ECabeceraEstadoResultadosIngreso>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();

                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "4.0.0";
                            break;
                        case 4:
                            codigoAuxac = "4.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "4.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "4.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "4.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxac
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraEstadoResultadosIngreso d = new ECabeceraEstadoResultadosIngreso();
                        d.Codigoingreso = cuentaabueloactivo.Codigo;
                        d.Cuentaingreso = cuentaabueloactivo.Nombre;


                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {
                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalactivo = d.Totalactivo + (j.MontoDebe-j.MontoHaber);

                                }
                                else
                                {
                                    d.Totalactivo = d.Totalactivo + (j.MontoDebeAlt-j.MontoHaberAlt);
                                }
                        }
                        cabeceraIngresos.Add(d);

                    }

                    return cabeceraIngresos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultado");
                }

            }
        }
        public List<ECabeceraEstadoResultadosCosto> cabeceraestadoresultadoscosto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<ECabeceraEstadoResultadosCosto> cabeceraCostos = new List<ECabeceraEstadoResultadosCosto>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();

                    string codigoAuxpas = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "5.1.0";
                            break;
                        case 4:
                            codigoAuxpas = "5.1.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "5.1.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "5.1.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "5.1.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 2 && x.Codigo == codigoAuxpas
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraEstadoResultadosCosto d = new ECabeceraEstadoResultadosCosto();
                        d.Codigocosto = cuentaabuelopasivo.Codigo;
                        d.Cuentacosto = cuentaabuelopasivo.Nombre;
                       


                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.MontoHaber > 0)
                            {
                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaber;

                                }
                                else
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaberAlt;
                                }
                            }


                        }
                        cabeceraCostos.Add(d);
                    }

                    return cabeceraCostos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultado");
                }

            }
        }
        public List<ECabeceraEstadoResultadosGasto> cabeceraestadoresultadosgasto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraEstadoResultadosGasto> cabeceraGastos = new List<ECabeceraEstadoResultadosGasto>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();

                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "5.2.0";
                            break;
                        case 4:
                            codigoAuxac = "5.2.0.0";
                            break;
                        case 5:
                            codigoAuxac = "5.2.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "5.2.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "5.2.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 2 && x.Codigo == codigoAuxac
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraEstadoResultadosGasto d = new ECabeceraEstadoResultadosGasto();
                        d.Codigogasto = cuentaabueloactivo.Codigo;
                        d.Cuentagasto = cuentaabueloactivo.Nombre;


                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.MontoDebe > 0)
                            {

                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoDebe;

                                }
                                else
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoDebeAlt;
                                }
                            }
                        }
                        cabeceraGastos.Add(d);

                    }

                    return cabeceraGastos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultados");
                }

            }
        }
        public List<EEstadoResultadosIngreso> reporteestadoresultadosingreso(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosIngreso> EstadoResultadosIngreso = new List<EEstadoResultadosIngreso>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "4.0.0";
                            break;
                        case 4:
                            codigoAuxac = "4.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "4.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "4.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "4.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivos = (from x in esquema.Cuenta
                                               where x.IdEmpresa == idempresa
                                               && x.Nivel == 1 && x.Codigo == codigoAuxac
                                               select x).ToList();

                    if (empresa.Niveles == 3)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {
                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();
                                    foreach (var j in detallecomprobante)
                                    {
                                        EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                        if (z.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.Nombre;
                                            d.CodigoCabecera1 = z.Codigo;
                                            d.IdCabecera1 = z.IdCuenta;


                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                            d.Cuenta = j.Cuenta.Nombre;

                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                            {
                                                d.Debe = j.MontoHaber - j.MontoDebe;
                                            }
                                            else
                                            {
                                                d.Debe = j.MontoHaberAlt - j.MontoDebeAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                            EstadoResultadosIngreso.Add(d);
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();

                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    foreach (var i in comprobante)
                                    {
                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();
                                        foreach (var j in detallecomprobante)
                                        {
                                            EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                            if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera5 = z.Nombre;
                                                d.CodigoCabecera5 = z.Codigo;
                                                d.IdCabecera5 = z.IdCuenta;
                                                d.CuentaCabecera1 = q.Nombre;
                                                d.CodigoCabecera1 = q.Codigo;
                                                d.IdCabecera1 = q.IdCuenta;


                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Debe = j.MontoHaber - j.MontoDebe;
                                                }
                                                else
                                                {
                                                    d.Debe = j.MontoHaberAlt - j.MontoDebeAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosIngreso.Add(d);
                                            }

                                        }

                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {
                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();

                                    foreach (var w in cuentas3)
                                    {

                                        foreach (var i in comprobante)
                                        {
                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();
                                            foreach (var j in detallecomprobante)
                                            {
                                                EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                                if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.IdCabecera5 = z.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;


                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Debe = j.MontoHaber - j.MontoDebe;
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.MontoHaberAlt - j.MontoDebeAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosIngreso.Add(d);

                                                }

                                            }

                                        }


                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {

                                            foreach (var i in comprobante)
                                            {
                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();
                                                foreach (var j in detallecomprobante)
                                                {
                                                    EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                                    if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.Nombre;
                                                        d.CodigoCabecera1 = e.Codigo;
                                                        d.IdCabecera1 = e.IdCuenta;
                                                        d.CuentaCabecera5 = z.Nombre;
                                                        d.CodigoCabecera5 = z.Codigo;
                                                        d.IdCabecera5 = z.IdCuenta;
                                                        d.CuentaCabecera4 = q.Nombre;
                                                        d.CodigoCabecera4 = q.Codigo;
                                                        d.IdCabecera4 = q.IdCuenta;
                                                        d.CuentaCabecera3 = w.Nombre;
                                                        d.CodigoCabecera3 = w.Codigo;
                                                        d.IdCabecera3 = w.IdCuenta;



                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Debe = j.MontoHaber - j.MontoDebe;
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.MontoHaberAlt - j.MontoDebeAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;


                                                        EstadoResultadosIngreso.Add(d);
                                                    }

                                                }

                                            }


                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {
                                            var cuentas5 = (from x in esquema.Cuenta
                                                            where x.IdEmpresa == idempresa
                                                            && x.Nivel == 6 && x.IdCuentaPadre == e.IdCuenta
                                                            select x).ToList();
                                            foreach (var r in cuentas5)
                                            {
                                                foreach (var i in comprobante)
                                                {
                                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                              where x.IdComprobante == i.IdComprobante
                                                                              select x).ToList();
                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EEstadoResultadosIngreso d = new EEstadoResultadosIngreso();
                                                        if (r.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = r.Nombre;
                                                            d.CodigoCabecera1 = r.Codigo;
                                                            d.IdCabecera1 = r.IdCuenta;
                                                            d.CuentaCabecera2 = e.Nombre;
                                                            d.CodigoCabecera2 = e.Codigo;
                                                            d.IdCabecera2 = e.IdCuenta;
                                                            d.CuentaCabecera3 = w.Nombre;
                                                            d.CodigoCabecera3 = w.Codigo;
                                                            d.IdCabecera3 = w.IdCuenta;
                                                            d.CuentaCabecera4 = q.Nombre;
                                                            d.CodigoCabecera4 = q.Codigo;
                                                            d.IdCabecera4 = q.IdCuenta;
                                                            d.CuentaCabecera5 = z.Nombre;
                                                            d.CodigoCabecera5 = z.Codigo;
                                                            d.IdCabecera5 = z.IdCuenta;


                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Debe = j.MontoHaber - j.MontoDebe;
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.MontoHaberAlt - j.MontoDebeAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;

                                                            EstadoResultadosIngreso.Add(d);


                                                        }

                                                    }

                                                }


                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }
                    return EstadoResultadosIngreso;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultados");
                }

            }
        }
        public List<EEstadoResultadosCosto> reporteestadoresultadoscosto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosCosto> EstadoResultadosCosto = new List<EEstadoResultadosCosto>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "5.1.0";
                            break;
                        case 4:
                            codigoAuxpas = "5.1.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "5.1.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "5.1.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "5.1.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 2 && x.Codigo == codigoAuxpas
                                                        select x).ToList();
                    if (empresa.Niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                           
                                foreach (var i in comprobante)
                                {

                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();

                                    foreach (var j in detallecomprobante)
                                    {
                                        EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                        if (l.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = l.Nombre;
                                            d.CodigoCabecera1 = l.Codigo;
                                            d.IdCabecera1 = l.IdCuenta;


                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                            d.Cuenta = j.Cuenta.Nombre;

                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                            {
                                                d.Haber = j.MontoDebe - j.MontoHaber;
                                            }
                                            else
                                            {
                                                d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                            EstadoResultadosCosto.Add(d);
                                        }
                                    }
                                }
                            
                        }
                    }
                    else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                              

                                    foreach (var i in comprobante)
                                    {

                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();

                                        foreach (var j in detallecomprobante)
                                        {
                                            EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                            if (l.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = z.Nombre;
                                                d.CodigoCabecera1 = z.Codigo;
                                                d.IdCabecera1 = z.IdCuenta;
                                                d.CuentaCabecera5 = l.Nombre;
                                                d.CodigoCabecera5 = l.Codigo;
                                                d.IdCabecera5 = l.IdCuenta;


                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.MontoDebe - j.MontoHaber;
                                                }
                                                else
                                                {
                                                    d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosCosto.Add(d);

                                            }
                                        }
                                    }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                   
                                        foreach (var i in comprobante)
                                        {

                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();

                                            foreach (var j in detallecomprobante)
                                            {
                                                EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                                if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = q.Nombre;
                                                    d.CodigoCabecera1 = q.Codigo;
                                                    d.IdCabecera1 = q.IdCuenta;
                                                    d.CuentaCabecera4 = z.Nombre;
                                                    d.CodigoCabecera4 = z.Codigo;
                                                    d.IdCabecera4 = z.IdCuenta;
                                                    d.CuentaCabecera5 = l.Nombre;
                                                    d.CodigoCabecera5 = l.Codigo;
                                                    d.IdCabecera5 = l.IdCuenta;


                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoDebe - j.MontoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosCosto.Add(d);

                                                }
                                            }
                                        }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 5 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                       
                                            foreach (var i in comprobante)
                                            {

                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();

                                                foreach (var j in detallecomprobante)
                                                {
                                                    EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                                    if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = w.Nombre;
                                                        d.CodigoCabecera1 = w.Codigo;
                                                        d.IdCabecera1 = w.IdCuenta;
                                                        d.CuentaCabecera3 = q.Nombre;
                                                        d.CodigoCabecera3 = q.Codigo;
                                                        d.IdCabecera3 = q.IdCuenta;
                                                        d.CuentaCabecera4 = z.Nombre;
                                                        d.CodigoCabecera4 = z.Codigo;
                                                        d.IdCabecera4 = z.IdCuenta;
                                                        d.CuentaCabecera5 = l.Nombre;
                                                        d.CodigoCabecera5 = l.Codigo;
                                                        d.IdCabecera5 = l.IdCuenta;


                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.MontoDebe - j.MontoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        EstadoResultadosCosto.Add(d);

                                                    }
                                                }
                                            }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 5 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 6 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            
                                                foreach (var i in comprobante)
                                                {

                                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                              where x.IdComprobante == i.IdComprobante
                                                                              select x).ToList();

                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EEstadoResultadosCosto d = new EEstadoResultadosCosto();
                                                        if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = e.Nombre;
                                                            d.CodigoCabecera1 = e.Codigo;
                                                            d.IdCabecera1 = e.IdCuenta;
                                                            d.CuentaCabecera2 = w.Nombre;
                                                            d.CodigoCabecera2 = w.Codigo;
                                                            d.IdCabecera2 = w.IdCuenta;
                                                            d.CuentaCabecera3 = q.Nombre;
                                                            d.CodigoCabecera3 = q.Codigo;
                                                            d.IdCabecera3 = q.IdCuenta;
                                                            d.CuentaCabecera4 = z.Nombre;
                                                            d.CodigoCabecera4 = z.Codigo;
                                                            d.IdCabecera4 = z.IdCuenta;
                                                            d.CuentaCabecera5 = l.Nombre;
                                                            d.CodigoCabecera5 = l.Codigo;
                                                            d.IdCabecera5 = l.IdCuenta;


                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Haber = j.MontoDebe - j.MontoHaber;
                                                            }
                                                            else
                                                            {
                                                                d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            EstadoResultadosCosto.Add(d);

                                                        }
                                                    }
                                                }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return EstadoResultadosCosto;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultados");
                }

            }
        }
        public List<EEstadoResultadosGasto> reporteestadoresultadosgasto(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EEstadoResultadosGasto> EstadoResultadosGasto = new List<EEstadoResultadosGasto>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpat = "5.2.0";
                            break;
                        case 4:
                            codigoAuxpat = "5.2.0.0";
                            break;
                        case 5:
                            codigoAuxpat = "5.2.0.0.0";
                            break;
                        case 6:
                            codigoAuxpat = "5.2.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpat = "5.2.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 2 && x.Codigo == codigoAuxpat
                                                        select x).ToList();
                    if (empresa.Niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {

                            foreach (var i in comprobante)
                            {

                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                          where x.IdComprobante == i.IdComprobante
                                                          select x).ToList();

                                foreach (var j in detallecomprobante)
                                {
                                    EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                    if (l.IdCuenta == j.Cuenta.IdCuentaPadre)
                                    {
                                        d.CuentaCabecera1 = l.Nombre;
                                        d.CodigoCabecera1 = l.Codigo;
                                        d.IdCabecera1 = l.IdCuenta;

                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                        d.Cuenta = j.Cuenta.Nombre;

                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                        {
                                            d.Haber = j.MontoDebe - j.MontoHaber;
                                        }
                                        else
                                        {
                                            d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                        }
                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                        EstadoResultadosGasto.Add(d);
                                    }
                                    
                                }
                            }

                        }
                    }
                    else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {


                                foreach (var i in comprobante)
                                {

                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();

                                    foreach (var j in detallecomprobante)
                                    {
                                        EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                        if (l.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.Nombre;
                                            d.CodigoCabecera1 = z.Codigo;
                                            d.IdCabecera1 = z.IdCuenta;
                                            d.CuentaCabecera5 = l.Nombre;
                                            d.CodigoCabecera5 = l.Codigo;
                                            d.IdCabecera5 = l.IdCuenta;


                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                            d.Cuenta = j.Cuenta.Nombre;

                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                            {
                                                d.Haber = j.MontoDebe - j.MontoHaber;
                                            }
                                            else
                                            {
                                                d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                            }
                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                            EstadoResultadosGasto.Add(d);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {

                                    foreach (var i in comprobante)
                                    {

                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();

                                        foreach (var j in detallecomprobante)
                                        {
                                            EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                            if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = q.Nombre;
                                                d.CodigoCabecera1 = q.Codigo;
                                                d.IdCabecera1 = q.IdCuenta;
                                                d.CuentaCabecera4 = z.Nombre;
                                                d.CodigoCabecera4 = z.Codigo;
                                                d.IdCabecera4 = z.IdCuenta;
                                                d.CuentaCabecera5 = l.Nombre;
                                                d.CodigoCabecera5 = l.Codigo;
                                                d.IdCabecera5 = l.IdCuenta;


                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.MontoDebe - j.MontoHaber;
                                                }
                                                else
                                                {
                                                    d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                EstadoResultadosGasto.Add(d);

                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 5 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {

                                        foreach (var i in comprobante)
                                        {

                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();

                                            foreach (var j in detallecomprobante)
                                            {
                                                EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                                if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;
                                                    d.CuentaCabecera3 = q.Nombre;
                                                    d.CodigoCabecera3 = q.Codigo;
                                                    d.IdCabecera3 = q.IdCuenta;
                                                    d.CuentaCabecera4 = z.Nombre;
                                                    d.CodigoCabecera4 = z.Codigo;
                                                    d.IdCabecera4 = z.IdCuenta;
                                                    d.CuentaCabecera5 = l.Nombre;
                                                    d.CodigoCabecera5 = l.Codigo;
                                                    d.IdCabecera5 = l.IdCuenta;


                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoDebe - j.MontoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                    d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    EstadoResultadosGasto.Add(d);

                                                }
                                            }
                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 3 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 4 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 5 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 6 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {

                                            foreach (var i in comprobante)
                                            {

                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();

                                                foreach (var j in detallecomprobante)
                                                {
                                                    EEstadoResultadosGasto d = new EEstadoResultadosGasto();
                                                    if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.Nombre;
                                                        d.CodigoCabecera1 = e.Codigo;
                                                        d.IdCabecera1 = e.IdCuenta;
                                                        d.CuentaCabecera2 = w.Nombre;
                                                        d.CodigoCabecera2 = w.Codigo;
                                                        d.IdCabecera2 = w.IdCuenta;
                                                        d.CuentaCabecera3 = q.Nombre;
                                                        d.CodigoCabecera3 = q.Codigo;
                                                        d.IdCabecera3 = q.IdCuenta;
                                                        d.CuentaCabecera4 = z.Nombre;
                                                        d.CodigoCabecera4 = z.Codigo;
                                                        d.IdCabecera4 = z.IdCuenta;
                                                        d.CuentaCabecera5 = l.Nombre;
                                                        d.CodigoCabecera5 = l.Codigo;
                                                        d.IdCabecera5 = l.IdCuenta;


                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.MontoDebe - j.MontoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.MontoDebeAlt - j.MontoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                        d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        EstadoResultadosGasto.Add(d);

                                                    }
                                                }
                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return EstadoResultadosGasto;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El Estado de Resultados");
                }

            }
        }
        //BALANCE GENERAL
        public List<ECabeceraGeneralActivo> cabeceraGeneralactivos(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraGeneralActivo> cabeceraActivos = new List<ECabeceraGeneralActivo>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();

                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxac
                                              select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraGeneralActivo d = new ECabeceraGeneralActivo();
                        d.Codigoactivo = cuentaabueloactivo.Codigo;
                        d.Cuentaactivo = cuentaabueloactivo.Nombre;


                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.MontoDebe > 0)
                            {

                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalactivo = d.Totalactivo + j.MontoDebe;

                                }
                                else
                                {
                                    d.Totalactivo = d.Totalactivo + j.MontoDebeAlt;
                                }
                            }
                        }
                        cabeceraActivos.Add(d);

                    }

                    return cabeceraActivos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance General");
                }

            }
        }
        public List<ECabeceraGeneralPasivoPatrimonio> cabeceraGeneralpasivopatrimonios(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ECabeceraGeneralPasivoPatrimonio> cabeceraPasivoPatrimonios = new List<ECabeceraGeneralPasivoPatrimonio>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();

                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivo = (from x in esquema.Cuenta
                                              where x.IdEmpresa == idempresa
                                              && x.Nivel == 1 && x.Codigo == codigoAuxpas
                                              select x).FirstOrDefault();
                    var cuentaabuelopatrimonio = (from x in esquema.Cuenta
                                                  where x.IdEmpresa == idempresa
                                                  && x.Nivel == 1 && x.Codigo == codigoAuxpat
                                                  select x).FirstOrDefault();
                    foreach (var i in comprobante)
                    {
                        ECabeceraGeneralPasivoPatrimonio d = new ECabeceraGeneralPasivoPatrimonio();
                        d.Codigopasivo = cuentaabuelopasivo.Codigo;
                        d.Cuentapasivo = cuentaabuelopasivo.Nombre;
                        d.Codigopatrimonio = cuentaabuelopatrimonio.Codigo;
                        d.Cuentapatrimonio = cuentaabuelopatrimonio.Nombre;


                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                  where x.IdComprobante == i.IdComprobante
                                                  select x).ToList();

                        foreach (var j in detallecomprobante)
                        {

                            if (j.MontoHaber > 0)
                            {
                                if (moneda.IdMonedaPrincipal == idmoneda)
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaber;

                                }
                                else
                                {
                                    d.Totalpasivopatrimonio = d.Totalpasivopatrimonio + j.MontoHaberAlt;
                                }
                            }


                        }
                        cabeceraPasivoPatrimonios.Add(d);
                    }

                    return cabeceraPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance General");
                }

            }
        }

        public List<EBalanceGeneralActivo> reporteBalanceGeneralActivo(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceGeneralActivo> balanceInicialActivos = new List<EBalanceGeneralActivo>();
                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();

                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxac = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxac = "1.0.0";
                            break;
                        case 4:
                            codigoAuxac = "1.0.0.0";
                            break;
                        case 5:
                            codigoAuxac = "1.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxac = "1.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxac = "1.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabueloactivos = (from x in esquema.Cuenta
                                               where x.IdEmpresa == idempresa
                                               && x.Nivel == 1 && x.Codigo == codigoAuxac
                                               select x).ToList();

                    if (empresa.Niveles == 3)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {
                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();
                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                        if (z.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.Nombre;
                                            d.CodigoCabecera1 = z.Codigo;
                                            d.IdCabecera1 = z.IdCuenta;

                                           
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Debe = j.MontoDebe-j.MontoHaber;
                                                }
                                                else
                                                {
                                                    d.Debe = j.MontoDebeAlt-j.MontoHaberAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                totalgeneral = totalgeneral + d.TotalCabecera1;
                                                balanceInicialActivos.Add(d);
                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();

                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    foreach (var i in comprobante)
                                    {
                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();
                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                            if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera5 = z.Nombre;
                                                d.CodigoCabecera5 = z.Codigo;
                                                d.IdCabecera5 = z.IdCuenta;
                                                d.CuentaCabecera1 = q.Nombre;
                                                d.CodigoCabecera1 = q.Codigo;
                                                d.IdCabecera1 = q.IdCuenta;

                                                
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Debe = j.MontoDebe-j.MontoHaber;
                                                    }
                                                    else
                                                    {
                                                        d.Debe = j.MontoDebeAlt-j.MontoHaberAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialActivos.Add(d);
                                            }

                                        }

                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {
                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();

                                    foreach (var w in cuentas3)
                                    {

                                        foreach (var i in comprobante)
                                        {
                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();
                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.IdCabecera5 = z.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;

                                                    
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Debe = j.MontoDebe-j.MontoHaber;
                                                        }
                                                        else
                                                        {
                                                            d.Debe = j.MontoDebeAlt-j.MontoHaberAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialActivos.Add(d);
                                                    
                                                }

                                            }

                                        }


                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {

                                            foreach (var i in comprobante)
                                            {
                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();
                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                    if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.Nombre;
                                                        d.CodigoCabecera1 = e.Codigo;
                                                        d.IdCabecera1 = e.IdCuenta;
                                                        d.CuentaCabecera5 = z.Nombre;
                                                        d.CodigoCabecera5 = z.Codigo;
                                                        d.IdCabecera5 = z.IdCuenta;
                                                        d.CuentaCabecera4 = q.Nombre;
                                                        d.CodigoCabecera4 = q.Codigo;
                                                        d.IdCabecera4 = q.IdCuenta;
                                                        d.CuentaCabecera3 = w.Nombre;
                                                        d.CodigoCabecera3 = w.Codigo;
                                                        d.IdCabecera3 = w.IdCuenta;


                                                       
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Debe = j.MontoDebe-j.MontoHaber;
                                                            }
                                                            else
                                                            {
                                                                d.Debe = j.MontoDebeAlt-j.MontoHaberAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;


                                                            balanceInicialActivos.Add(d);
                                                    }

                                                }

                                            }


                                        }

                                    }

                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabueloactivos)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas2 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas2)
                                {

                                    var cuentas3 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas3)
                                    {
                                        var cuentas4 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas4)
                                        {
                                            var cuentas5 = (from x in esquema.Cuenta
                                                            where x.IdEmpresa == idempresa
                                                            && x.Nivel == 6 && x.IdCuentaPadre == e.IdCuenta
                                                            select x).ToList();
                                            foreach (var r in cuentas5)
                                            {
                                                foreach (var i in comprobante)
                                                {
                                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                              where x.IdComprobante == i.IdComprobante
                                                                              select x).ToList();
                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EBalanceGeneralActivo d = new EBalanceGeneralActivo();
                                                        if (r.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = r.Nombre;
                                                            d.CodigoCabecera1 = r.Codigo;
                                                            d.IdCabecera1 = r.IdCuenta;
                                                            d.CuentaCabecera2 = e.Nombre;
                                                            d.CodigoCabecera2 = e.Codigo;
                                                            d.IdCabecera2 = e.IdCuenta;
                                                            d.CuentaCabecera3 = w.Nombre;
                                                            d.CodigoCabecera3 = w.Codigo;
                                                            d.IdCabecera3 = w.IdCuenta;
                                                            d.CuentaCabecera4 = q.Nombre;
                                                            d.CodigoCabecera4 = q.Codigo;
                                                            d.IdCabecera4 = q.IdCuenta;
                                                            d.CuentaCabecera5 = z.Nombre;
                                                            d.CodigoCabecera5 = z.Codigo;
                                                            d.IdCabecera5 = z.IdCuenta;

                                                            
                                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                                d.Cuenta = j.Cuenta.Nombre;

                                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                                {
                                                                    d.Debe = j.MontoDebe-j.MontoHaber;
                                                                }
                                                                else
                                                                {
                                                                    d.Debe = j.MontoDebeAlt-j.MontoHaberAlt;
                                                                }
                                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Debe;
                                                                d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                                d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;

                                                                balanceInicialActivos.Add(d);
                                                            

                                                        }

                                                    }

                                                }


                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }
                    return balanceInicialActivos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance General");
                }

            }
        }
        public List<EBalanceGeneralPasivoPatrimonio> reporteBalanceGeneralPasivoPatrimonio(int idgestion, int idempresa, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<EBalanceGeneralPasivoPatrimonio> balanceInicialPasivoPatrimonios = new List<EBalanceGeneralPasivoPatrimonio>();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();
                    var comprobante = (from x in esquema.Comprobante
                                       where x.IdEmpresa == idempresa
                                       && x.Fecha >= gestion.Fechainicio && x.Fecha <= gestion.Fechafin
                                       && x.Estado != (int)EstadoComprobante.Anualdo
                                       select x).ToList();
                    double totalcabecera = 0;
                    double totalgeneral = 0;
                    string codigoAuxpas = "";
                    string codigoAuxpat = "";
                    switch (empresa.Niveles)
                    {
                        case 3:
                            codigoAuxpas = "2.0.0";
                            codigoAuxpat = "3.0.0";
                            break;
                        case 4:
                            codigoAuxpas = "2.0.0.0";
                            codigoAuxpat = "3.0.0.0";
                            break;
                        case 5:
                            codigoAuxpas = "2.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0";
                            break;
                        case 6:
                            codigoAuxpas = "2.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0";
                            break;
                        case 7:
                            codigoAuxpas = "2.0.0.0.0.0.0";
                            codigoAuxpat = "3.0.0.0.0.0.0";
                            break;

                        default:
                            break;
                    }
                    var cuentaabuelopasivopatrimonio = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 1 && x.Codigo == codigoAuxpas || x.Codigo == codigoAuxpat
                                                        select x).ToList();
                    if (empresa.Niveles == 3)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {

                                foreach (var i in comprobante)
                                {

                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                              where x.IdComprobante == i.IdComprobante
                                                              select x).ToList();

                                    foreach (var j in detallecomprobante)
                                    {
                                        EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                        if (z.IdCuenta == j.Cuenta.IdCuentaPadre)
                                        {
                                            d.CuentaCabecera1 = z.Nombre;
                                            d.CodigoCabecera1 = z.Codigo;
                                            d.IdCabecera1 = z.IdCuenta;

                                            
                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                d.Cuenta = j.Cuenta.Nombre;

                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                {
                                                    d.Haber = j.MontoHaber-j.MontoDebe;
                                                }
                                                else
                                                {
                                                    d.Haber = j.MontoHaberAlt-j.MontoDebeAlt;
                                                }
                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera1;
                                                balanceInicialPasivoPatrimonios.Add(d);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 4)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {

                                    foreach (var i in comprobante)
                                    {

                                        var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                  where x.IdComprobante == i.IdComprobante
                                                                  select x).ToList();

                                        foreach (var j in detallecomprobante)
                                        {
                                            EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                            if (q.IdCuenta == j.Cuenta.IdCuentaPadre)
                                            {
                                                d.CuentaCabecera1 = q.Nombre;
                                                d.CodigoCabecera1 = q.Codigo;
                                                d.IdCabecera1 = q.IdCuenta;
                                                d.CuentaCabecera5 = z.Nombre;
                                                d.CodigoCabecera5 = z.Codigo;
                                                d.IdCabecera5 = z.IdCuenta;

                                                
                                                    d.CodigoCuenta = j.Cuenta.Codigo;
                                                    d.Cuenta = j.Cuenta.Nombre;

                                                    if (moneda.IdMonedaPrincipal == idmoneda)
                                                    {
                                                        d.Haber = j.MontoHaber-j.MontoDebe;
                                                    }
                                                    else
                                                    {
                                                        d.Haber = j.MontoHaberAlt-j.MontoDebeAlt;
                                                    }
                                                    d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                    d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera1;
                                                    d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                    balanceInicialPasivoPatrimonios.Add(d);
                                                
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 5)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        foreach (var i in comprobante)
                                        {

                                            var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                      where x.IdComprobante == i.IdComprobante
                                                                      select x).ToList();

                                            foreach (var j in detallecomprobante)
                                            {
                                                EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                if (w.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                {
                                                    d.CuentaCabecera1 = w.Nombre;
                                                    d.CodigoCabecera1 = w.Codigo;
                                                    d.IdCabecera1 = w.IdCuenta;
                                                    d.CuentaCabecera4 = q.Nombre;
                                                    d.CodigoCabecera4 = q.Codigo;
                                                    d.IdCabecera4 = q.IdCuenta;
                                                    d.CuentaCabecera5 = z.Nombre;
                                                    d.CodigoCabecera5 = z.Codigo;
                                                    d.IdCabecera5 = z.IdCuenta;

                                                    
                                                        d.CodigoCuenta = j.Cuenta.Codigo;
                                                        d.Cuenta = j.Cuenta.Nombre;

                                                        if (moneda.IdMonedaPrincipal == idmoneda)
                                                        {
                                                            d.Haber = j.MontoHaber-j.MontoDebe;
                                                        }
                                                        else
                                                        {
                                                            d.Haber = j.MontoHaberAlt-j.MontoDebeAlt;
                                                        }
                                                        d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                        d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera1;
                                                        d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                        d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                        balanceInicialPasivoPatrimonios.Add(d);
                                                  
                                                }
                                            }
                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 6)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            foreach (var i in comprobante)
                                            {

                                                var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                          where x.IdComprobante == i.IdComprobante
                                                                          select x).ToList();

                                                foreach (var j in detallecomprobante)
                                                {
                                                    EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                    if (e.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                    {
                                                        d.CuentaCabecera1 = e.Nombre;
                                                        d.CodigoCabecera1 = e.Codigo;
                                                        d.IdCabecera1 = e.IdCuenta;
                                                        d.CuentaCabecera3 = w.Nombre;
                                                        d.CodigoCabecera3 = w.Codigo;
                                                        d.IdCabecera3 = w.IdCuenta;
                                                        d.CuentaCabecera4 = q.Nombre;
                                                        d.CodigoCabecera4 = q.Codigo;
                                                        d.IdCabecera4 = q.IdCuenta;
                                                        d.CuentaCabecera5 = z.Nombre;
                                                        d.CodigoCabecera5 = z.Codigo;
                                                        d.IdCabecera5 = z.IdCuenta;

                                                       
                                                            d.CodigoCuenta = j.Cuenta.Codigo;
                                                            d.Cuenta = j.Cuenta.Nombre;

                                                            if (moneda.IdMonedaPrincipal == idmoneda)
                                                            {
                                                                d.Haber = j.MontoHaber-j.MontoDebe;
                                                            }
                                                            else
                                                            {
                                                                d.Haber = j.MontoHaberAlt-j.MontoDebeAlt;
                                                            }
                                                            d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                            d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera1;
                                                            d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                            d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                            d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                            balanceInicialPasivoPatrimonios.Add(d);
                                                       
                                                    }
                                                }
                                            }


                                        }


                                    }


                                }
                            }
                        }
                    }
                    else if (empresa.Niveles == 7)
                    {
                        foreach (var l in cuentaabuelopasivopatrimonio)
                        {
                            var cuentas = (from x in esquema.Cuenta
                                           where x.IdEmpresa == idempresa
                                           && x.Nivel == 2 && x.IdCuentaPadre == l.IdCuenta
                                           select x).ToList();
                            foreach (var z in cuentas)
                            {
                                var cuentas1 = (from x in esquema.Cuenta
                                                where x.IdEmpresa == idempresa
                                                && x.Nivel == 3 && x.IdCuentaPadre == z.IdCuenta
                                                select x).ToList();
                                foreach (var q in cuentas1)
                                {
                                    var cuentas2 = (from x in esquema.Cuenta
                                                    where x.IdEmpresa == idempresa
                                                    && x.Nivel == 4 && x.IdCuentaPadre == q.IdCuenta
                                                    select x).ToList();
                                    foreach (var w in cuentas2)
                                    {
                                        var cuentas3 = (from x in esquema.Cuenta
                                                        where x.IdEmpresa == idempresa
                                                        && x.Nivel == 5 && x.IdCuentaPadre == w.IdCuenta
                                                        select x).ToList();
                                        foreach (var e in cuentas3)
                                        {
                                            var cuentas4 = (from x in esquema.Cuenta
                                                            where x.IdEmpresa == idempresa
                                                            && x.Nivel == 6 && x.IdCuentaPadre == e.IdCuenta
                                                            select x).ToList();
                                            foreach (var r in cuentas4)
                                            {
                                                foreach (var i in comprobante)
                                                {

                                                    var detallecomprobante = (from x in esquema.DetalleComprobante
                                                                              where x.IdComprobante == i.IdComprobante
                                                                              select x).ToList();

                                                    foreach (var j in detallecomprobante)
                                                    {
                                                        EBalanceGeneralPasivoPatrimonio d = new EBalanceGeneralPasivoPatrimonio();
                                                        if (r.IdCuenta == j.Cuenta.IdCuentaPadre)
                                                        {
                                                            d.CuentaCabecera1 = r.Nombre;
                                                            d.CodigoCabecera1 = r.Codigo;
                                                            d.IdCabecera1 = r.IdCuenta;
                                                            d.CuentaCabecera2 = e.Nombre;
                                                            d.CodigoCabecera2 = e.Codigo;
                                                            d.IdCabecera2 = e.IdCuenta;
                                                            d.CuentaCabecera3 = w.Nombre;
                                                            d.CodigoCabecera3 = w.Codigo;
                                                            d.IdCabecera3 = w.IdCuenta;
                                                            d.CuentaCabecera4 = q.Nombre;
                                                            d.CodigoCabecera4 = q.Codigo;
                                                            d.IdCabecera4 = q.IdCuenta;
                                                            d.CuentaCabecera5 = z.Nombre;
                                                            d.CodigoCabecera5 = z.Codigo;
                                                            d.IdCabecera5 = z.IdCuenta;

                                                           
                                                                d.CodigoCuenta = j.Cuenta.Codigo;
                                                                d.Cuenta = j.Cuenta.Nombre;

                                                                if (moneda.IdMonedaPrincipal == idmoneda)
                                                                {
                                                                    d.Haber = j.MontoHaber-j.MontoDebe;
                                                                }
                                                                else
                                                                {
                                                                    d.Haber = j.MontoHaberAlt-j.MontoDebeAlt;
                                                                }
                                                                d.TotalCabecera1 = d.TotalCabecera1 + d.Haber;
                                                                d.TotalCabecera2 = d.TotalCabecera2 + d.TotalCabecera1;
                                                                d.TotalCabecera3 = d.TotalCabecera3 + d.TotalCabecera2;
                                                                d.TotalCabecera4 = d.TotalCabecera4 + d.TotalCabecera3;
                                                                d.TotalCabecera5 = d.TotalCabecera5 + d.TotalCabecera4;
                                                                d.TotalGeneral = d.TotalGeneral + d.TotalCabecera5;
                                                                balanceInicialPasivoPatrimonios.Add(d);
                                                            
                                                        }
                                                    }
                                                }


                                            }

                                        }


                                    }


                                }
                            }
                        }
                    }

                    return balanceInicialPasivoPatrimonios;

                }
                catch (Exception ex)
                {
                    throw new MensageException("no se pudo obtener El balance General");
                }

            }
        }
        public List<ERDatosBasicoBalanceGeneral> ReporteDatosBasicoBalanceGeneral(string usuario, string empresa, int idmoneda, int idgestion)
        {
            try
            {

                List<ERDatosBasicoBalanceGeneral> basicos = new List<ERDatosBasicoBalanceGeneral>();
                ERDatosBasicoBalanceGeneral eRDatosBasicoBalance = new ERDatosBasicoBalanceGeneral();

                var moneda = LMoneda.Logica.LMoneda.obtenerMoneda(idmoneda);
                var gestion = LGestion.Logica.LGestion.obtenerGestion(idgestion);


                eRDatosBasicoBalance.Usuario = usuario;
                eRDatosBasicoBalance.NombreEmpresa = empresa;
                eRDatosBasicoBalance.NombreGestion = gestion.Nombre;
                eRDatosBasicoBalance.FechaInicioGestion = gestion.Fechainicio.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaFinGestion = gestion.Fechafin.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.Moneda = moneda.Nombre;

                basicos.Add(eRDatosBasicoBalance);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error logica.");
            }
        }
        public List<ERDatosBasicoEstadoResultados> ReporteDatosBasicoEstadoResultados(string usuario, string empresa, int idmoneda, int idgestion)
        {
            try
            {

                List<ERDatosBasicoEstadoResultados> basicos = new List<ERDatosBasicoEstadoResultados>();
                ERDatosBasicoEstadoResultados eRDatosBasicoBalance = new ERDatosBasicoEstadoResultados();

                var moneda = LMoneda.Logica.LMoneda.obtenerMoneda(idmoneda);
                var gestion = LGestion.Logica.LGestion.obtenerGestion(idgestion);


                eRDatosBasicoBalance.Usuario = usuario;
                eRDatosBasicoBalance.NombreEmpresa = empresa;
                eRDatosBasicoBalance.NombreGestion = gestion.Nombre;
                eRDatosBasicoBalance.FechaInicioGestion = gestion.Fechainicio.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaFinGestion = gestion.Fechafin.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                eRDatosBasicoBalance.Moneda = moneda.Nombre;

                basicos.Add(eRDatosBasicoBalance);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error logica.");
            }
        }
    }
}
