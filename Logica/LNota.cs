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
    public class LNota : LLogica<Nota>
    {
        public List<Nota> listarNota(int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<Nota> nota = new List<Nota>();


                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa && x.Tipo == (int)TipoNota.Compra
                                 orderby x.NroNota descending
                                 select x).ToList();
                    foreach (var i in notas)
                    {
                        Nota c = new Nota();
                        c.IdNota = i.IdNota;
                        c.Lote = i.Lote;
                        c.Empresa = i.Empresa;
                        c.Estado = i.Estado;
                        c.Fecha = i.Fecha;
                        c.Descripcion = i.Descripcion;
                        c.IdEmpresa = i.IdEmpresa;
                        c.NroNota = i.NroNota;
                        c.IdUsuario = i.IdUsuario;
                        c.Total = i.Total;
                        c.Tipo = i.Tipo;
                        c.Usuario = i.Usuario;

                        nota.Add(c);
                    }


                    return nota;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Notas");
                }

            }
        }
        public List<Nota> listarNota2(int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    List<Nota> nota = new List<Nota>();


                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa && x.Tipo == (int)TipoNota.Venta
                                 orderby x.NroNota descending
                                 select x).ToList();
                    foreach (var i in notas)
                    {
                        Nota c = new Nota();
                        c.IdNota = i.IdNota;
                        c.Lote = i.Lote;
                        c.Empresa = i.Empresa;
                        c.Estado = i.Estado;
                        c.Fecha = i.Fecha;
                        c.Descripcion = i.Descripcion;
                        c.IdEmpresa = i.IdEmpresa;
                        c.NroNota = i.NroNota;
                        c.IdUsuario = i.IdUsuario;
                        c.Total = i.Total;
                        c.Tipo = i.Tipo;
                        c.Usuario = i.Usuario;

                        nota.Add(c);
                    }


                    return nota;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Notas");
                }

            }
        }
        public ERNotasVenta obtenerNotasVenta(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    
                    var gestion= (from x in esquema.Gestion
                                  where x.IdEmpresa == idempresa
                                  && x.Estado == (int)EstadoGestion.Abierto
                                  select x).ToList();
                    ERNotasVenta enotas = new ERNotasVenta();
                    enotas.Periodo = new List<EPeriodosJSON>();
                    enotas.Nota = new List<ENotasJSON>();
                    foreach (var g in gestion)
                    {
                        var periodo = (from x in esquema.Periodo
                                       where x.IdGestion == g.IdGestion
                                       orderby x.Fechainicio ascending
                                       select x).ToList();

                        if (periodo != null)
                        {

                            

                            foreach (var p in periodo)
                            {
                                double contador = 0;
                                var notas = (from x in esquema.Nota
                                             where x.IdEmpresa == idempresa
                                             && x.Tipo == (int)TipoNota.Venta
                                             && x.Estado == (int)EstadoNota.Activo
                                             && x.Fecha <= p.Fechafin && x.Fecha >= p.Fechainicio
                                             select x).ToList();
                                if (notas != null)
                                {
                                    foreach(var n in notas)
                                    {
                                        contador = contador + n.Total;
                                    }
                                    ENotasJSON z = new ENotasJSON();
                                    z.Total = contador;
                                    enotas.Nota.Add(z);
                                }
                                EPeriodosJSON c = new EPeriodosJSON();
                                c.Descripcion = p.Nombre;
                                enotas.Periodo.Add(c);
                            }

                        }
                        else
                        {
                            EPeriodosJSON c = new EPeriodosJSON();
                            c.Descripcion = "Sin periodos";
                            enotas.Periodo.Add(c);
                            ENotasJSON z = new ENotasJSON();
                            z.Total = 0;
                            enotas.Nota.Add(z);

                        }
                    }
                    return enotas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Notas");
                }

            }
        }
        public ERGraficos2 obtenerProductosBajos(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var productos = (from x in esquema.Articulo
                                   where x.IdEmpresa == idempresa
                                   && x.Cantidad  < 50
                                   select x).ToList();
                    ERGraficos2 enotas = new ERGraficos2();
                    enotas.Producto = new List<EProductosBajoStockJSON>();
                   
                        if (productos != null)
                        {
                            foreach (var p in productos)
                            {
                                    EProductosBajoStockJSON z = new EProductosBajoStockJSON();
                                    z.Descripcion = p.Nombre;
                                    z.Stock=p.Cantidad;
                                    enotas.Producto.Add(z);
                               
                            }

                        }
                        else
                        {
                                    EProductosBajoStockJSON z = new EProductosBajoStockJSON();
                                    z.Descripcion = "" ;
                                    z.Stock=0;
                                    enotas.Producto.Add(z);

                        }
                    
                        return enotas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de productos");
                }

            }
        }
        public Nota ObtenerNota(int idempresa, int idnota)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Nota c = new Nota();


                    var notas = (from x in esquema.Nota
                                        where x.IdEmpresa == idempresa
                                        && x.IdNota == idnota
                                        select x).FirstOrDefault();


                    if (notas != null)
                    {
                        c.Lote = notas.Lote;
                        c.Empresa = notas.Empresa;
                        c.Estado = notas.Estado;
                        c.Fecha = notas.Fecha;
                        c.Descripcion = notas.Descripcion;
                        c.IdNota = notas.IdNota;
                        c.IdEmpresa = notas.IdEmpresa;
                        c.Tipo = notas.Tipo;
                        c.IdUsuario = notas.IdUsuario;
                        c.Total = notas.Total;
                        c.NroNota = notas.NroNota;
                        c.Usuario = notas.Usuario;
                    }
                    else
                    {
                        throw new MensageException("Error no se pudo obtener la Nota");
                    }
                    return c;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se pudo obtener la Nota");
                }

            }
        }

        public List<ELote> listarLoteXNota(int idnota, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    List<ELote> Lote = new List<ELote>();


                    var lnota = (from x in esquema.Lote
                                         where x.IdNota == idnota
                                         select x).ToList();
                    

                    foreach (var i in lnota)
                    {
                        var larticulo = (from x in esquema.Articulo
                                         where x.IdArticulo == i.IdArticulo
                                         select x).ToList();
                        foreach(var a in larticulo)
                        {
                            ELote e = new ELote();

                            e.IdArticulo = i.IdArticulo;
                            e.Articulo = a.Nombre;
                            e.FechaIngreso = i.FechaIngreso.ToString();
                            e.FechaVencimiento = i.FechaVencimiento.ToString();
                            e.Cantidad = i.Cantidad;
                            e.PrecioCompra = i.PrecioCompra;
                            e.Stock = i.Stock;
                            e.SubTotal =i.Cantidad*i.PrecioCompra;
                            e.Estado = i.Estado;

                            Lote.Add(e);
                        }
                       

                    }

                    return Lote;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener el lote de la nota");
                }

            }
        }
        public List<EDetalle> listarDetalleXNota(int idnota, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    List<EDetalle> Detalle = new List<EDetalle>();


                    var lnota = (from x in esquema.Detalle
                                 where x.IdNota == idnota
                                 select x).ToList();


                    foreach (var i in lnota)
                    {
                        var larticulo = (from x in esquema.Articulo
                                         where x.IdArticulo == i.IdArticulo
                                         select x).ToList();
                        foreach (var a in larticulo)
                        {
                            EDetalle e = new EDetalle();

                            e.IdArticulo = i.IdArticulo;
                            e.Articulo = a.Nombre;
                            e.NroLote = i.NroLote;
                            e.Cantidad = i.Cantidad;
                            e.SubTotal = i.Cantidad * i.PrecioVenta;
                            e.PrecioVenta = i.PrecioVenta;

                            Detalle.Add(e);
                        }


                    }

                    return Detalle;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener el Detalle de la nota");
                }

            }
        }

        public List<ENotaTotal> TotalLote(List<ELote> loteNotas)
        {
            try
            {
                List<ENotaTotal> eDetalleSubTotals = new List<ENotaTotal>();
                ENotaTotal detalleSubTotal = new ENotaTotal();
                detalleSubTotal.Total = 0;


                foreach (var i in loteNotas)
                {
                    detalleSubTotal.Total = detalleSubTotal.Total+i.SubTotal;
                }

                eDetalleSubTotals.Add(detalleSubTotal);

                return eDetalleSubTotals;


            }
            catch (Exception ex)
            {
                throw new MensageException("Error no se puedo obtener la lista de lotes de nota");
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

        public int obtenerNroNotaVenta(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    int Serie = 1;
                    var notas = (from x in esquema.Nota
                                        where x.IdEmpresa == idempresa && x.Tipo==(int)TipoNota.Venta
                                        orderby x.NroNota descending
                                        select x).FirstOrDefault();

                    if (notas != null)
                    {
                        Serie = Serie + notas.NroNota;
                    }
                    return Serie;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error No se pudo obtener El numero de nota");
                }

            }
        }
        public int obtenerNroNotaCompra(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    int Serie = 1;
                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa && x.Tipo == (int)TipoNota.Compra
                                 orderby x.NroNota descending
                                 select x).FirstOrDefault();

                    if (notas != null)
                    {
                        Serie = Serie + notas.NroNota;
                    }
                    return Serie;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error No se pudo obtener El numero de nota");
                }

            }
        }

        //sin modificar

        public Nota Registro(Nota Entidad, List<ELote> eDetalleComprobante, ENotaTotal DetalleTotal)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);
                    ValidacionExisteDetalle(DetalleTotal);
                    var empresa= (from x in esquema.Empresa
                                          where x.IdEmpresa == Entidad.IdEmpresa
                                          select x).FirstOrDefault();
                    if (empresa.Integracion == true)
                    {
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
                                var serie = LComprobante.Logica.LComprobante.obtenerSerie(Entidad.IdEmpresa);
                                Comprobante c = new Comprobante();
                                c.IdEmpresa = empresa.IdEmpresa;
                                c.Serie = serie;
                                c.Glosa = "Compra de Mercaderias";
                                c.Fecha = Entidad.Fecha;
                                if (moneda.Cambio == null)
                                {
                                    c.TipoCambio = 0;
                                }
                                else
                                {
                                    c.TipoCambio = moneda.Cambio.Value;
                                }
                                c.Estado = (int)EstadoComprobante.Abierto;
                                c.TipoComprobante = (int)TipoComprobante.Egreso;
                                c.IdUsuario = Entidad.IdUsuario;
                                c.IdMoneda = moneda.IdMonedaPrincipal;
                                esquema.Comprobante.Add(c);
                                esquema.SaveChanges();
                                    
                                        DetalleComprobante d = new DetalleComprobante();
                                        d.Numero = 1;
                                        d.Glosa =  "Compra de Mercaderias";
                                        d.IdCuenta = empresa.IdCuentaCompras.Value;
                                        d.IdComprobante = c.IdComprobante;
                                        d.IdUsuario = Entidad.IdUsuario;
                                        
                                        if (moneda.Cambio == null)
                                        {
                                                d.MontoDebe =(DetalleTotal.Total-(DetalleTotal.Total*0.13));
                                                d.MontoHaber = 0;

                                                d.MontoDebeAlt = 0;
                                                d.MontoHaberAlt = 0;
                                        }
                                        else
                                        {
                                                d.MontoDebe = (DetalleTotal.Total-(DetalleTotal.Total*0.13));
                                                d.MontoHaber = 0;

                                                d.MontoDebeAlt = Math.Round(((DetalleTotal.Total-(DetalleTotal.Total*0.13))/(moneda.Cambio.Value)), 2);
                                                d.MontoHaberAlt = 0;

                                        }
                                        esquema.DetalleComprobante.Add(d);
                                DetalleComprobante z = new DetalleComprobante();
                                z.Numero = 2;
                                z.Glosa = "Compra de Mercaderias";
                                z.IdCuenta = empresa.IdCuentaCreditoFiscal.Value;
                                z.IdComprobante = c.IdComprobante;
                                z.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    z.MontoDebe = DetalleTotal.Total * 0.13;
                                    z.MontoHaber = 0;

                                    z.MontoDebeAlt = 0;
                                    z.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    z.MontoDebe = DetalleTotal.Total * 0.13;
                                    z.MontoHaber = 0;

                                    z.MontoDebeAlt = Math.Round(((DetalleTotal.Total * 0.13)/(moneda.Cambio.Value)), 2);
                                    z.MontoHaberAlt = 0;

                                }
                                esquema.DetalleComprobante.Add(z);
                                DetalleComprobante x = new DetalleComprobante();
                                x.Numero = 3;
                                x.Glosa = "Compra de Mercaderias";
                                x.IdCuenta = empresa.IdCuentaCaja.Value;
                                x.IdComprobante = c.IdComprobante;
                                x.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    x.MontoDebe =0 ;
                                    x.MontoHaber = DetalleTotal.Total;

                                    x.MontoDebeAlt = 0;
                                    x.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    x.MontoDebe = 0;
                                    x.MontoHaber = DetalleTotal.Total;

                                    x.MontoDebeAlt = 0;
                                    x.MontoHaberAlt = Math.Round(((DetalleTotal.Total) / (moneda.Cambio.Value)), 2);

                                }
                                esquema.DetalleComprobante.Add(x);
                                esquema.SaveChanges();
                                if (Entidad.Tipo == (int)TipoNota.Compra)
                                {

                                    var notas = (from n in esquema.Nota
                                                 where n.IdEmpresa == Entidad.IdEmpresa
                                                 select n).ToList();
                                    Entidad.Total = DetalleTotal.Total;
                                    Entidad.IdComprobante = c.IdComprobante;
                                    esquema.Nota.Add(Entidad);
                                    esquema.SaveChanges();
                                    if (notas.Count() == 0)
                                    {
                                        foreach (var i in eDetalleComprobante)
                                        {

                                            Lote o = new Lote();

                                            o.NroLote = 1;
                                            o.FechaIngreso = Entidad.Fecha;
                                            if (i.FechaVencimiento == "")
                                            {
                                                o.FechaVencimiento = null;
                                            }
                                            else
                                            {
                                                o.FechaVencimiento = Convert.ToDateTime(i.FechaVencimiento);
                                            }
                                            o.IdArticulo = i.IdArticulo;
                                            o.IdNota = Entidad.IdNota;
                                            o.Cantidad = i.Cantidad;
                                            o.PrecioCompra = i.PrecioCompra;
                                            o.Stock = i.Cantidad;
                                            o.Estado = (int)EstadoLote.Activo;
                                            esquema.Lote.Add(o);
                                            var articulomod = (from u in esquema.Articulo
                                                               where u.IdArticulo == i.IdArticulo
                                                               select u).FirstOrDefault();
                                            articulomod.Cantidad = articulomod.Cantidad + i.Cantidad;
                                            esquema.SaveChanges();


                                        }
                                    }
                                    else if (notas.Count() > 0)
                                    {

                                        foreach (var i in eDetalleComprobante)
                                        {
                                            int nrolote = 0;
                                            foreach (var n in notas)
                                            {
                                                var lote = (from y in esquema.Lote
                                                            where y.IdNota == n.IdNota
                                                            select y).ToList();
                                                foreach (var l in lote)
                                                {
                                                    if (i.IdArticulo == l.IdArticulo)
                                                    {
                                                        nrolote = nrolote + 1;
                                                    }
                                                }
                                            }
                                            Lote t = new Lote();

                                            t.NroLote = nrolote + 1;
                                            t.FechaIngreso = Entidad.Fecha;
                                            if (i.FechaVencimiento == "")
                                            {
                                                t.FechaVencimiento = null;
                                            }
                                            else
                                            {
                                                t.FechaVencimiento = Convert.ToDateTime(i.FechaVencimiento);
                                            }
                                            t.IdArticulo = i.IdArticulo;
                                            t.IdNota = Entidad.IdNota;
                                            t.Cantidad = i.Cantidad;
                                            t.PrecioCompra = i.PrecioCompra;
                                            t.Stock = i.Cantidad;
                                            t.Estado = (int)EstadoLote.Activo;
                                            esquema.Lote.Add(t);
                                            var articulomod = (from r in esquema.Articulo
                                                               where r.IdArticulo == i.IdArticulo
                                                               select r).FirstOrDefault();
                                            articulomod.Cantidad = articulomod.Cantidad + i.Cantidad;
                                            esquema.SaveChanges();
                                        }

                                    }


                                }
                                else
                                {
                                    throw new MensageException("No se puede guardar");
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
                    }
                    else
                    {
                        if (Entidad.Tipo == (int)TipoNota.Compra)
                        {

                            var notas = (from x in esquema.Nota
                                         where x.IdEmpresa == Entidad.IdEmpresa
                                         select x).ToList();
                            Entidad.Total = DetalleTotal.Total;
                            esquema.Nota.Add(Entidad);
                            esquema.SaveChanges();
                            if (notas.Count() == 0)
                            {
                                foreach (var i in eDetalleComprobante)
                                {

                                    Lote d = new Lote();

                                    d.NroLote = 1;
                                    d.FechaIngreso = Entidad.Fecha;
                                    if (i.FechaVencimiento == "")
                                    {
                                        d.FechaVencimiento = null;
                                    }
                                    else
                                    {
                                        d.FechaVencimiento = Convert.ToDateTime(i.FechaVencimiento);
                                    }
                                    d.IdArticulo = i.IdArticulo;
                                    d.IdNota = Entidad.IdNota;
                                    d.Cantidad = i.Cantidad;
                                    d.PrecioCompra = i.PrecioCompra;
                                    d.Stock = i.Cantidad;
                                    d.Estado = (int)EstadoLote.Activo;
                                    esquema.Lote.Add(d);
                                    var articulomod = (from x in esquema.Articulo
                                                       where x.IdArticulo == i.IdArticulo
                                                       select x).FirstOrDefault();
                                    articulomod.Cantidad = articulomod.Cantidad + i.Cantidad;
                                    esquema.SaveChanges();


                                }
                            }
                            else if (notas.Count() > 0)
                            {

                                foreach (var i in eDetalleComprobante)
                                {
                                    int nrolote = 0;
                                    foreach (var n in notas)
                                    {
                                        var lote = (from x in esquema.Lote
                                                    where x.IdNota == n.IdNota
                                                    select x).ToList();
                                        foreach (var l in lote)
                                        {
                                            if (i.IdArticulo == l.IdArticulo)
                                            {
                                                nrolote = nrolote + 1;
                                            }
                                        }
                                    }
                                    Lote d = new Lote();

                                    d.NroLote = nrolote + 1;
                                    d.FechaIngreso = Entidad.Fecha;
                                    if (i.FechaVencimiento == "")
                                    {
                                        d.FechaVencimiento = null;
                                    }
                                    else
                                    {
                                        d.FechaVencimiento = Convert.ToDateTime(i.FechaVencimiento);
                                    }
                                    d.IdArticulo = i.IdArticulo;
                                    d.IdNota = Entidad.IdNota;
                                    d.Cantidad = i.Cantidad;
                                    d.PrecioCompra = i.PrecioCompra;
                                    d.Stock = i.Cantidad;
                                    d.Estado = (int)EstadoLote.Activo;
                                    esquema.Lote.Add(d);
                                    var articulomod = (from x in esquema.Articulo
                                                       where x.IdArticulo == i.IdArticulo
                                                       select x).FirstOrDefault();
                                    articulomod.Cantidad = articulomod.Cantidad + i.Cantidad;
                                    esquema.SaveChanges();
                                }

                            }


                        }
                        else
                        {
                            throw new MensageException("No se puede guardar");
                        }
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
        public Nota RegistroVenta(Nota Entidad, List<EDetalle> eDetalleComprobante, ENotaTotal DetalleTotal)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);
                    ValidacionExisteDetalle(DetalleTotal);
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == Entidad.IdEmpresa
                                   select x).FirstOrDefault();
                    if (empresa.Integracion == true)
                    {
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
                                var serie = LComprobante.Logica.LComprobante.obtenerSerie(Entidad.IdEmpresa);
                                Comprobante c = new Comprobante();
                                c.IdEmpresa = empresa.IdEmpresa;
                                c.Serie = serie;
                                c.Glosa = "Venta de Mercaderias";
                                c.Fecha = Entidad.Fecha;
                                if (moneda.Cambio == null)
                                {
                                    c.TipoCambio = 0;
                                }
                                else
                                {
                                    c.TipoCambio = moneda.Cambio.Value;
                                }
                                c.Estado = (int)EstadoComprobante.Abierto;
                                c.TipoComprobante = (int)TipoComprobante.Ingreso;
                                c.IdUsuario = Entidad.IdUsuario;
                                c.IdMoneda = moneda.IdMonedaPrincipal;
                                esquema.Comprobante.Add(c);
                                esquema.SaveChanges();

                                DetalleComprobante d = new DetalleComprobante();
                                d.Numero = 1;
                                d.Glosa = "Venta de Mercaderias";
                                d.IdCuenta = empresa.IdCuentaCaja.Value;
                                d.IdComprobante = c.IdComprobante;
                                d.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    d.MontoDebe = DetalleTotal.Total;
                                    d.MontoHaber = 0;

                                    d.MontoDebeAlt = 0;
                                    d.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    d.MontoDebe = DetalleTotal.Total;
                                    d.MontoHaber = 0;

                                    d.MontoDebeAlt = Math.Round(((DetalleTotal.Total) / (moneda.Cambio.Value)), 2);
                                    d.MontoHaberAlt = 0;

                                }
                                esquema.DetalleComprobante.Add(d);
                                DetalleComprobante z = new DetalleComprobante();
                                z.Numero = 2;
                                z.Glosa = "Venta de Mercaderias";
                                z.IdCuenta = empresa.IdCuentaIt.Value;
                                z.IdComprobante = c.IdComprobante;
                                z.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    z.MontoDebe = DetalleTotal.Total * 0.03;
                                    z.MontoHaber = 0;

                                    z.MontoDebeAlt = 0;
                                    z.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    z.MontoDebe = DetalleTotal.Total * 0.03;
                                    z.MontoHaber = 0;

                                    z.MontoDebeAlt = Math.Round(((DetalleTotal.Total * 0.03) / (moneda.Cambio.Value)), 2);
                                    z.MontoHaberAlt = 0;

                                }
                                esquema.DetalleComprobante.Add(z);
                                DetalleComprobante x = new DetalleComprobante();
                                x.Numero = 3;
                                x.Glosa = "Venta de Mercaderias";
                                x.IdCuenta = empresa.IdCuentaVentas.Value;
                                x.IdComprobante = c.IdComprobante;
                                x.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    x.MontoDebe = 0;
                                    x.MontoHaber = ((DetalleTotal.Total)-(DetalleTotal.Total*0.13));

                                    x.MontoDebeAlt = 0;
                                    x.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    x.MontoDebe = 0;
                                    x.MontoHaber = ((DetalleTotal.Total) - (DetalleTotal.Total * 0.13));

                                    x.MontoDebeAlt = 0;
                                    x.MontoHaberAlt = Math.Round((((DetalleTotal.Total)-(DetalleTotal.Total * 0.13)) /(moneda.Cambio.Value)), 2);

                                }
                                esquema.DetalleComprobante.Add(x);
                                DetalleComprobante v = new DetalleComprobante();
                                v.Numero = 4;
                                v.Glosa = "Venta de Mercaderias";
                                v.IdCuenta = empresa.IdCuentaDebitoFiscal.Value;
                                v.IdComprobante = c.IdComprobante;
                                v.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    v.MontoDebe = 0;
                                    v.MontoHaber = DetalleTotal.Total * 0.13;

                                    v.MontoDebeAlt = 0;
                                    v.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    v.MontoDebe = 0;
                                    v.MontoHaber = DetalleTotal.Total * 0.13;

                                    v.MontoDebeAlt = 0;
                                    v.MontoHaberAlt = Math.Round(((DetalleTotal.Total * 0.13) / (moneda.Cambio.Value)), 2);

                                }
                                esquema.DetalleComprobante.Add(v);
                                DetalleComprobante b = new DetalleComprobante();
                                b.Numero = 5;
                                b.Glosa = "Venta de Mercaderias";
                                b.IdCuenta = empresa.IdCuentaItPorPagar.Value;
                                b.IdComprobante = c.IdComprobante;
                                b.IdUsuario = Entidad.IdUsuario;

                                if (moneda.Cambio == null)
                                {
                                    b.MontoDebe =0 ;
                                    b.MontoHaber = DetalleTotal.Total * 0.03;

                                    b.MontoDebeAlt = 0;
                                    b.MontoHaberAlt = 0;
                                }
                                else
                                {
                                    b.MontoDebe = 0;
                                    b.MontoHaber = DetalleTotal.Total * 0.03;

                                    b.MontoDebeAlt =0 ;
                                    b.MontoHaberAlt = Math.Round(((DetalleTotal.Total * 0.03) / (moneda.Cambio.Value)), 2);

                                }
                                esquema.DetalleComprobante.Add(b);
                                esquema.SaveChanges();
                                if (Entidad.Tipo == (int)TipoNota.Venta)
                                {

                                    var notas = (from s in esquema.Nota
                                                 where s.IdEmpresa == Entidad.IdEmpresa
                                                 && s.Tipo == (int)TipoNota.Compra
                                                 && s.Estado == (int)EstadoNota.Activo
                                                 select s).ToList();

                                    Entidad.Total = DetalleTotal.Total;
                                    Entidad.IdComprobante = c.IdComprobante;
                                    esquema.Nota.Add(Entidad);
                                    esquema.SaveChanges();

                                    foreach (var i in eDetalleComprobante)
                                    {

                                        Detalle n = new Detalle();

                                        n.IdArticulo = i.IdArticulo;
                                        n.NroLote = i.NroLote;
                                        n.IdNota = Entidad.IdNota;
                                        n.Cantidad = i.Cantidad;
                                        n.PrecioVenta = i.PrecioVenta;
                                        esquema.Detalle.Add(n);
                                        var articulomod = (from h in esquema.Articulo
                                                           where h.IdArticulo == i.IdArticulo
                                                           select h).FirstOrDefault();
                                        articulomod.Cantidad = articulomod.Cantidad - i.Cantidad;
                                        var lotes = (from h in esquema.Lote
                                                     where h.IdArticulo == i.IdArticulo && h.NroLote == i.NroLote
                                                     select h).FirstOrDefault();
                                        lotes.Stock = lotes.Stock - i.Cantidad;
                                        esquema.SaveChanges();
                                    }
                                    foreach (var n in notas)
                                    {
                                        var ajuste = (from q in esquema.Lote
                                                      where q.IdNota == n.IdNota && q.Estado == (int)EstadoLote.Activo
                                                      select q).ToList();
                                        foreach (var a in ajuste)
                                        {
                                            if (a.Stock == 0)
                                            {
                                                a.Estado = (int)EstadoLote.Agotado;
                                                esquema.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    throw new MensageException("No se puede guardar");
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
                    }
                    else
                    {
                        if (Entidad.Tipo == (int)TipoNota.Venta)
                        {

                            var notas = (from x in esquema.Nota
                                         where x.IdEmpresa == Entidad.IdEmpresa
                                         && x.Tipo == (int)TipoNota.Compra
                                         && x.Estado == (int)EstadoNota.Activo
                                         select x).ToList();

                            Entidad.Total = DetalleTotal.Total;
                            esquema.Nota.Add(Entidad);
                            esquema.SaveChanges();

                            foreach (var i in eDetalleComprobante)
                            {

                                Detalle d = new Detalle();

                                d.IdArticulo = i.IdArticulo;
                                d.NroLote = i.NroLote;
                                d.IdNota = Entidad.IdNota;
                                d.Cantidad = i.Cantidad;
                                d.PrecioVenta = i.PrecioVenta;
                                esquema.Detalle.Add(d);
                                var articulomod = (from x in esquema.Articulo
                                                   where x.IdArticulo == i.IdArticulo
                                                   select x).FirstOrDefault();
                                articulomod.Cantidad = articulomod.Cantidad - i.Cantidad;
                                var lotes = (from x in esquema.Lote
                                             where x.IdArticulo == i.IdArticulo && x.NroLote == i.NroLote
                                             select x).FirstOrDefault();
                                lotes.Stock = lotes.Stock - i.Cantidad;
                                esquema.SaveChanges();
                            }
                            foreach (var n in notas)
                            {
                                var ajuste = (from x in esquema.Lote
                                              where x.IdNota == n.IdNota && x.Estado == (int)EstadoLote.Activo
                                              select x).ToList();
                                foreach (var a in ajuste)
                                {
                                    if (a.Stock == 0)
                                    {
                                        a.Estado = (int)EstadoLote.Agotado;
                                        esquema.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new MensageException("No se puede guardar");
                        }
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

        public Nota AnularNota(int idnota)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    
                    var nota = (from x in esquema.Nota
                                       where x.IdNota == idnota
                                       select x).FirstOrDefault();
                    var validacionlotes= (from vl in esquema.Lote
                                          where vl.IdNota == idnota
                                          select vl).ToList();
                    int contador = 0;
                    foreach(var vlotes in validacionlotes)
                    {
                        var validaciondetalles = (from d in esquema.Detalle
                                               where d.IdArticulo == vlotes.IdArticulo && d.NroLote==vlotes.NroLote
                                               select d).ToList();
                        if(validaciondetalles.Count() > 0)
                        {
                            contador = contador + 1;
                        }
                    }
                    
                    if (nota != null)
                    { 
                        if(contador == 0)
                        {
                            nota.Estado = (int)EstadoNota.Anulado;
                            esquema.SaveChanges();
                            var lote = (from x in esquema.Lote
                                        where x.IdNota == nota.IdNota
                                        select x).ToList();
                            foreach (var i in lote)
                            {
                                var articulos = (from a in esquema.Articulo
                                                 where a.IdArticulo == i.IdArticulo
                                                 select a).FirstOrDefault();
                                articulos.Cantidad = articulos.Cantidad - i.Cantidad;
                                i.Estado = (int)EstadoNota.Anulado;
                                esquema.SaveChanges();
                            }
                            var comprobante= (from x in esquema.Comprobante
                                              where x.IdComprobante == nota.IdComprobante
                                              select x).FirstOrDefault();
                            if (comprobante != null)
                            {
                                comprobante.Estado = (int)EstadoComprobante.Anualdo;
                                esquema.SaveChanges();
                            }
                        }
                        else
                        {
                            throw new MensageException("No se puede anular la nota porque los lotes ya estan en una venta");
                        }
                           
                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener la nota");
                    }

                    return nota;
                }
                catch (MensageException ex)
                {
                    throw new MensageException(ex.Message);
                }
            }

        }
        public Nota AnularNotaVenta(int idnota,int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    
                    var nota = (from x in esquema.Nota
                                where x.IdNota == idnota
                                select x).FirstOrDefault();
                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa
                                 && x.Tipo == (int)TipoNota.Compra
                                 && x.Estado == (int)EstadoNota.Activo
                                 select x).ToList();

                    if (nota != null)
                    {
                        nota.Estado = (int)EstadoNota.Anulado;
                        esquema.SaveChanges();
                        var detalle = (from x in esquema.Detalle
                                    where x.IdNota == nota.IdNota
                                    select x).ToList();
                        foreach (var i in detalle)
                        {
                            var articulo = (from a in esquema.Articulo
                                            where a.IdArticulo == i.IdArticulo
                                            select a).FirstOrDefault();

                            var lotes= (from l in esquema.Lote
                                           where l.NroLote == i.NroLote && l.IdArticulo==i.IdArticulo
                                           select l).FirstOrDefault();
                            
                                lotes.Stock = lotes.Stock + i.Cantidad;
                                articulo.Cantidad = articulo.Cantidad + i.Cantidad;
                                esquema.SaveChanges();

                        }
                        
                        foreach (var n in notas)
                        {
                            var ajuste = (from x in esquema.Lote
                                          where x.IdNota == n.IdNota && x.Estado == (int)EstadoLote.Agotado
                                          select x).ToList();
                            foreach (var a in ajuste)
                            {
                                if (a.Stock > 0)
                                {
                                    a.Estado = (int)EstadoLote.Activo;
                                    esquema.SaveChanges();
                                }
                            }
                        }
                        var comprobante = (from x in esquema.Comprobante
                                           where x.IdComprobante == nota.IdComprobante
                                           select x).FirstOrDefault();
                        if (comprobante != null)
                        {
                            comprobante.Estado = (int)EstadoComprobante.Anualdo;
                            esquema.SaveChanges();
                        }

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener la nota");
                    }

                    return nota;
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

        public void Validacion(Nota Entidad)
        {
            if (Entidad.Tipo==0)
            {
                throw new MensageException("Seleccione Un tipo de Nota");
            }

            if (string.IsNullOrEmpty(Entidad.Descripcion))
            {
                throw new MensageException("Ingrese una descripcion");
            }

        }
        public void ValidacionDetalleTotal(EDetalleTotal Entidad)
        {
            if (Entidad.TotalDebe != Entidad.TotalHaber)
            {
                throw new MensageException("Los Totales No Son Iguales");
            }
        }
        public void ValidacionExisteDetalle(ENotaTotal Entidad)
        {
            if (Entidad.Total == 0)
            {
                throw new MensageException("Deben haber al menos 1 detalle registrado");
            }

        }
        //reportes
        public List<ERDatosBasicoCuenta> ReporteDatosBasicoNota(string usuario, string empresa)
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
        public List<ENota> ObtenerNotaReporte(int idempresa, int idnota)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    List<ENota> eComprobantes = new List<ENota>();
                    ENota c = new ENota();


                    var comprobantes = (from x in esquema.Nota
                                        where x.IdEmpresa == idempresa
                                        && x.IdNota == idnota
                                        select x).FirstOrDefault();
                    

                    if (comprobantes != null)
                    {
                        
                        c.NroNota = comprobantes.NroNota;
                        c.Descripcion = comprobantes.Descripcion;
                        c.Total = comprobantes.Total;
                        if (comprobantes.Tipo == (int)TipoNota.Compra)
                        {
                            c.TipoNota = "Compra";
                        }
                        else if(comprobantes.Tipo == (int)TipoNota.Venta)
                        {
                            c.TipoNota = "Venta";
                        }
                        
                        c.Fecha = comprobantes.Fecha.ToString("dd/MM/yyyy");

                        switch (comprobantes.Estado)
                        {
                            case (int)EstadoNota.Activo:
                                c.Estado = "Activo";
                                break;
                            case (int)EstadoNota.Anulado:
                                c.Estado = "Anulado";
                                break;
                        }
                    }
                    else
                    {
                        throw new MensageException("Error no se puedo obtener la Nota");
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
        public List<ELote> listarDetalleNotaCompraXNota(int idnota, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    List<ELote> detalleComprobantes = new List<ELote>();


                    var dcomprobantes = (from x in esquema.Lote
                                         where x.IdNota == idnota
                                         select x).ToList();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    foreach (var i in dcomprobantes)
                    {
                        var producto= (from x in esquema.Articulo
                                       where x.IdArticulo == i.IdArticulo
                                       select x).FirstOrDefault();

                        ELote e = new ELote();
                        e.IdArticulo = i.IdArticulo;
                        e.Articulo = producto.Nombre;
                        e.NroLote = i.NroLote;
                        e.FechaIngreso = i.FechaIngreso.ToString("dd/MM/yyyy");
                        if (i.FechaVencimiento == null)
                        {
                            e.FechaVencimiento = "Sin Fecha";
                        }
                        else
                        {
                            e.FechaVencimiento = i.FechaVencimiento.ToString();
                        }
                        
                        e.Cantidad = i.Cantidad;
                        e.PrecioCompra = i.PrecioCompra;
                        e.SubTotal = i.Cantidad * i.PrecioCompra;
                        detalleComprobantes.Add(e);

                    }
                    return detalleComprobantes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle de nota");
                }

            }
        }
        public List<EDetalle> listarDetalleNotaVentaXNota(int idnota, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    List<EDetalle> detalleComprobantes = new List<EDetalle>();


                    var dcomprobantes = (from x in esquema.Detalle
                                         where x.IdNota == idnota
                                         select x).ToList();

                    var moneda = LMoneda.Logica.LMoneda.obtenerMonedadXempresa(idempresa);

                    foreach (var i in dcomprobantes)
                    {
                        var producto = (from x in esquema.Articulo
                                        where x.IdArticulo == i.IdArticulo
                                        select x).FirstOrDefault();

                        EDetalle e = new EDetalle();
                        e.IdArticulo = i.IdArticulo;
                        e.Articulo = producto.Nombre;
                        e.NroLote = i.NroLote;
                        e.Cantidad = i.Cantidad;
                        e.PrecioVenta = i.PrecioVenta;
                        e.SubTotal = i.Cantidad * i.PrecioVenta;
                        detalleComprobantes.Add(e);

                    }
                    return detalleComprobantes;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de detalle de nota");
                }

            }
        }

    }
}
