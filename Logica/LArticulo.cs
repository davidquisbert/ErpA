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
    public class LArticulo : LLogica<Articulo>
    {
        public List<Articulo> listarArticulo(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var periodo = (from x in esquema.Articulo
                                   where x.IdEmpresa == idempresa
                                   select x).ToList();

                    List<Articulo> periodos = new List<Articulo>();

                    periodos = periodo;

                    return periodos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }

        public List<Articulo> listarArticuloConLotes(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulo = (from x in esquema.Articulo
                                   where x.IdEmpresa == idempresa
                                   select x).ToList();
                    var nota = (from x in esquema.Nota
                                    where x.IdEmpresa == idempresa && x.Estado==(int)EstadoNota.Activo
                                    select x).ToList();
                    List<Articulo> articulos = new List<Articulo>();
                    foreach (var i in nota)
                    {
                        var lote = (from x in esquema.Lote
                                    where x.IdNota == i.IdNota && x.Estado==(int)EstadoLote.Activo
                                    select x).ToList();
                        foreach(var l in lote)
                        {
                            foreach(var a in articulo)
                            {
                                if (a.IdArticulo == l.IdArticulo)
                                {
                                    int contador = 0;
                                    foreach (var repart in articulos){
                                        
                                        if (a.IdArticulo == repart.IdArticulo)
                                        {
                                            contador = contador + 1;
                                        }
                                    }
                                    if (contador == 0)
                                    {
                                        Articulo earticulo = new Articulo();
                                        earticulo.IdArticulo = a.IdArticulo;
                                        earticulo.Nombre = a.Nombre;
                                        earticulo.Cantidad = a.Cantidad;
                                        earticulo.PrecioVenta = a.PrecioVenta;
                                        articulos.Add(earticulo);
                                    }
                                    
                                }
                            }
                        }
                    }

                    return articulos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }
        public ERArticulo obtenerArticulo(int idarticulo,int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var articulo = (from x in esquema.Articulo
                                   where x.IdArticulo == idarticulo
                                   && x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();
                    var articulocat = (from x in esquema.ArticuloCategoria
                                    where x.IdArticulo == idarticulo
                                    select x).ToList();

                    if (articulo != null)
                    {

                        ERArticulo earticulo = new ERArticulo();

                        earticulo.IdArticulo = articulo.IdArticulo;
                        earticulo.NombreArticulo = articulo.Nombre;
                        earticulo.Descripcion = articulo.Descripcion;
                        earticulo.PrecioVenta = articulo.PrecioVenta;

                        earticulo.Categoria = new List<ECategoriaJSON>();

                        foreach (var i in articulocat)
                        {
                            ECategoriaJSON c = new ECategoriaJSON();
                            c.IdCategoria = i.IdCategoria;
                            earticulo.Categoria.Add(c);
                        }

                        return earticulo;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el articulo");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Articulos");
                }

            }
        }
        public ERArticuloLote obtenerLote(int idarticulo, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var notas = (from x in esquema.Nota
                                    where x.IdEmpresa == idempresa
                                    select x).ToList();
                    if (notas != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        foreach (var i in notas)
                        {
                            var lotes= (from x in esquema.Lote
                                        where x.IdNota == i.IdNota
                                        select x).ToList();
                            foreach(var l in lotes)
                            {
                                if (l.IdArticulo == idarticulo)
                                {
                                    ELoteJSON c = new ELoteJSON();
                                    c.NroLote = l.NroLote;
                                    c.FechaIngreso = l.FechaIngreso.ToString("dd/MM/yyyy");
                                    c.FechaVencimiento = l.FechaVencimiento.ToString();
                                    c.Cantidad = l.Cantidad;
                                    c.Stock = l.Stock;
                                    c.Estado = l.Estado;
                                    elote.Lote.Add(c);
                                }
                            }
                        }
                    
                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }
        public ERArticuloLote obtenerLoteArticulo(int nrolote, int idarticulo)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var lote = (from x in esquema.Lote
                                 where x.IdArticulo == idarticulo && x.NroLote==nrolote && x.Estado==(int)EstadoLote.Activo
                                 select x).FirstOrDefault();
                    if (lote != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        ELoteJSON c = new ELoteJSON();
                        c.NroLote = lote.NroLote;
                        c.Cantidad = lote.Cantidad;
                        c.Stock = lote.Stock;
                        c.PrecioCompra = lote.PrecioCompra;
                        elote.Lote.Add(c);

                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }
        
        public ERArticuloLote obtenerLoteActivo(int idarticulo, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {

                    var notas = (from x in esquema.Nota
                                 where x.IdEmpresa == idempresa && x.Estado==(int)EstadoNota.Activo
                                 select x).ToList();
                    if (notas != null)
                    {
                        ERArticuloLote elote = new ERArticuloLote();
                        elote.Lote = new List<ELoteJSON>();
                        foreach (var i in notas)
                        {
                            var lotes = (from x in esquema.Lote
                                         where x.IdNota == i.IdNota && x.Estado==(int)EstadoLote.Activo
                                         select x).ToList();
                            foreach (var l in lotes)
                            {
                                if (l.IdArticulo == idarticulo)
                                {
                                    ELoteJSON c = new ELoteJSON();
                                    c.NroLote = l.NroLote;
                                    c.FechaIngreso = l.FechaIngreso.ToString("dd/MM/yyyy");
                                    c.FechaVencimiento = l.FechaVencimiento.ToString();
                                    c.Cantidad = l.Cantidad;
                                    c.Stock = l.Stock;
                                    c.Estado = l.Estado;
                                    elote.Lote.Add(c);
                                }
                            }
                        }

                        return elote;

                    }
                    else
                    {
                        throw new MensageException("No se pudo obtener el lote");
                    }

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Lotes");
                }

            }
        }

        public Articulo Registro(Articulo Entidad, List<ECategoriaJSON> categoriaJSON)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (categoriaJSON == null)
                    {
                        throw new MensageException("Selecciones almenos una categoria");
                    }
                    Validacion(Entidad);


                    var empresa = LEmpresa.Logica.LEmpresa.obtenerEmpresa(Entidad.IdEmpresa);

                    if (empresa == null)
                    {
                        throw new MensageException("No se pudo encontrar la Empresa");
                    }

                    /*  var listaperiodo = (from x in esquema.Periodo
                                          where x.Estado == (int)EstadoPeriodo.Abierto
                                           && x.IdGestion == Entidad.IdGestion
                                          select x).ToList();

                     if (listaperiodo.Count() > 1)
                      {
                          throw new MensageException("Ya existe 2 periodos abiertos, no es posible registrar otra periodo");
                      }*/

                    Articulo periodoexiste = (from x in esquema.Articulo
                                             where x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);

                    

                    esquema.Articulo.Add(Entidad);
                    esquema.SaveChanges();

                    List<ArticuloCategoria> categorias = new List<ArticuloCategoria>();

                    foreach (var i in categoriaJSON)
                    {

                        ArticuloCategoria categoria = new ArticuloCategoria();

                        categoria.IdCategoria = i.IdCategoria;
                        categoria.IdArticulo = Entidad.IdArticulo;

                        esquema.ArticuloCategoria.Add(categoria);
                        esquema.SaveChanges();

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

        public Articulo Editar(Articulo Entidad, List<ECategoriaJSON> categoriaJSON)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (categoriaJSON == null)
                    {
                        throw new MensageException("Seleccione almenos una categoria");
                    }
                    Validacion(Entidad);

                    var empresa = LEmpresa.Logica.LEmpresa.obtenerEmpresa(Entidad.IdEmpresa);

                    if (empresa == null)
                    {
                        throw new MensageException("No se pudo encontrar la Empresa");
                    }

                    
                    Articulo periodoexiste = (from x in esquema.Articulo
                                             where x.IdArticulo != Entidad.IdArticulo
                                             && x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    var articulo = (from x in esquema.Articulo
                                    where x.IdArticulo == Entidad.IdArticulo && x.IdEmpresa == empresa.IdEmpresa
                                    select x).FirstOrDefault();

                   


                    if (articulo == null)
                    {
                        throw new MensageException("No se puedo obtener el Articulo");

                    }
                    else
                    {
                        var articulocat = (from x in esquema.ArticuloCategoria
                                           where x.IdArticulo == Entidad.IdArticulo
                                           select x).ToList();

                        List<ArticuloCategoria> auxiliar = new List<ArticuloCategoria>();
                        foreach (var i in articulocat)
                        {
                            ArticuloCategoria c = new ArticuloCategoria();

                            c = i;
                            esquema.ArticuloCategoria.Remove(c);
                        }

                        articulo.Nombre = Entidad.Nombre;
                        articulo.Descripcion = Entidad.Descripcion;
                        articulo.PrecioVenta = Entidad.PrecioVenta;
                        articulo.IdUsuario = Entidad.IdUsuario;
                        foreach (var q in categoriaJSON)
                        {
                            ArticuloCategoria categoria = new ArticuloCategoria();

                            categoria.IdCategoria = q.IdCategoria;
                            categoria.IdArticulo = Entidad.IdArticulo;

                            esquema.ArticuloCategoria.Add(categoria);
                        }
                        esquema.SaveChanges();

                        

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

        public void Eliminar(int idarticulo,int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var periodo = (from x in esquema.Articulo
                                   where x.IdArticulo == idarticulo
                                   select x).FirstOrDefault();
                    var articulocat = (from x in esquema.ArticuloCategoria
                                   where x.IdArticulo == idarticulo
                                   select x).ToList();
                    var notas= (from x in esquema.Nota
                                where x.IdEmpresa == idempresa
                                select x).ToList();
                    int contador = 0;
                    foreach (var i in notas)
                    {
                        var lotes = (from x in esquema.Lote
                                     where x.IdNota == i.IdNota && x.IdArticulo==idarticulo 
                                     select x).ToList();
                        if(lotes.Count() > 0)
                        {
                            contador = contador + 1;
                        }
                        
                    }
                    

                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el Articulo");
                    }
                    if (articulocat == null)
                    {
                        throw new MensageException("No se puede eliminar");
                    }
                    if(contador > 0)
                    {
                        throw new MensageException("No se puede eliminar este articulo porque ya fue registrado en una nota");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/
                    foreach(var i in articulocat)
                    {
                        ArticuloCategoria c = new ArticuloCategoria();
                        c = i;
                        esquema.ArticuloCategoria.Remove(c);
                    }
                    
                    esquema.Articulo.Remove(periodo);
                    esquema.SaveChanges();
                    // }

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

       /* public List<ERDatosBasicoPeriodo> ReporteDatosBasicoPeriodo(string usuario, string empresa, string gestion)
        {
            try
            {


                List<ERDatosBasicoPeriodo> basicos = new List<ERDatosBasicoPeriodo>();
                ERDatosBasicoPeriodo eRDatosBasico = new ERDatosBasicoPeriodo();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.NombreGestion = gestion;
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
       */

        public void Validacion(Articulo Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("Ingrese un Nombre.");
            }

            if (double.IsNaN(Entidad.PrecioVenta))
            {
                throw new MensageException("ingrese un precio de venta");
            }

        }


        public void validarExistencia(Articulo Existe, Articulo Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya Existe este articulo en la empresa.");
                }
            }
        }

        /*public List<ERPeriodo> ReportePeriodo(int idgestion)
        {
            List<ERPeriodo> periodos = new List<ERPeriodo>();

            try
            {
                using (var esquema = GetEsquema())
                {
                    var periodo = (from x in esquema.Periodo
                                   where x.IdGestion == idgestion
                                   orderby x.Fechainicio
                                   select x).ToList();



                    foreach (var i in periodo)
                    {
                        ERPeriodo g = new ERPeriodo();
                        g.NombrePeriodo = i.Nombre;
                        g.FechaInicio = i.Fechainicio.ToString("dd/MM/yyyy");
                        g.FechaFin = i.Fechafin.ToString("dd/MM/yyyy");
                        if (i.Estado == (int)EstadoGestion.Abierto)
                        {
                            g.Estado = "Abierta";
                        }
                        else if (i.Estado == (int)EstadoGestion.Cerrado)
                        {
                            g.Estado = "Cerrado";
                        }
                        else
                        {
                            g.Estado = "";
                        }

                        periodos.Add(g);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new MensageException("Error no se puedo obtener el reporte de periodos");
            }
        


            return periodos;
        }
        */
    }
}
