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
    public class LCategoria : LLogica<Categoria>
    {

        public List<ECategoria> listarCategorias(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdEmpresa == idempresa
                                      && x.IdCategoriaPadre == null
                                      select x).ToList();

                    List<ECategoria> categorias = new List<ECategoria>();


                    foreach (var i in linqcuenta)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.id = i.IdCategoria;
                        categoria.idCategoria = i.IdCategoria;
                        categoria.Nombre = i.Nombre;
                        categoria.text = i.Nombre;
                        categoria.IdUsuario = i.IdUsuario;
                        categoria.IdEmpresa = i.IdEmpresa;
                        categoria.IdCategoriaPadre = i.IdCategoriaPadre;
                        categoria.children = CategoriaHijos(categoria.idCategoria, idempresa);
                        categorias.Add(categoria);
                    }

                    //  empresas = empresa;

                    return categorias;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de empresas");
                }



            }
        }
        public List<ECategoria> listarCategoriasArticulos(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdEmpresa == idempresa
                                      select x).ToList();

                    List<ECategoria> categorias = new List<ECategoria>();


                    foreach (var i in linqcuenta)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.id = i.IdCategoria;
                        categoria.idCategoria = i.IdCategoria;
                        categoria.Nombre = i.Nombre;
                        categoria.text = i.Nombre;
                        categoria.IdUsuario = i.IdUsuario;
                        categoria.IdEmpresa = i.IdEmpresa;
                        categoria.IdCategoriaPadre = i.IdCategoriaPadre;
                        categoria.children = CategoriaHijos(categoria.idCategoria, idempresa);
                        categorias.Add(categoria);
                    }

                    //  empresas = empresa;

                    return categorias;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Categorias");
                }



            }
        }
        public Categoria obtenerCategoria(int idcategoria)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdCategoria == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("No se obtuvo la Categoria");
                    }

                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException("No se obtuvo la Categoria");
                }

            }
        }

       /* public List<Cuenta> listarCuentaDetalle(int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var cuentas = (from x in esquema.Cuenta
                                   where x.IdEmpresa == idempresa
                                   && x.TipoDeCuenta == (int)TipoCuenta.Detalle
                                   orderby x.Codigo ascending
                                   select x).ToList();

                    return cuentas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la cuenta");
                }

            }
        }*/

        public Categoria EliminarCategoria(int idcategoria)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdCategoria == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("Error no se pudo obtener la Categoria");
                    }


                    if (linqcuenta.Categoria1.Count() > 0)
                    {
                        throw new MensageException("No se puede eliminar la Categoria de esta empresa");

                    }

                    /* if(linqcuenta.IdCuentaPadre != null)
                     {
                         throw new MensageException("No se puede eliminar la cuenta de esta empresa esta relacionada");
                     }*/


                    esquema.Categoria.Remove(linqcuenta);
                    esquema.SaveChanges();


                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException(ex.Message);
                }

            }
        }


       /* public List<ECuenta> listarCuentasReporte(int idusuario, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdEmpresa == idempresa
                                      orderby x.Codigo ascending
                                      select x).ToList();

                    //List<Empresa> empresas = new List<Empresa>();


                    List<ECuenta> cuentas = new List<ECuenta>();


                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.id = i.IdCuenta;
                        cuenta.idCuenta = i.IdCuenta;
                        cuenta.Codigo = i.Codigo;
                        cuenta.Nombre = i.Nombre;
                        if (i.Nivel == 1)
                        {
                            cuenta.text = "" + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 2)
                        {
                            cuenta.text = "        " + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 3)
                        {
                            cuenta.text = "        " + "        " + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 4)
                        {
                            cuenta.text = "        " + "        " + "        " + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 5)
                        {
                            cuenta.text = "        " + "        " + "        " + "        " + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 6)
                        {
                            cuenta.text = "        " + "        " + "        " + "        " + "        " + i.Codigo + " " + i.Nombre;
                        }
                        else if (i.Nivel == 7)
                        {
                            cuenta.text = "        " + "        " + "        " + "        " + "        " + "        " + i.Codigo + " " + i.Nombre;
                        }
                        cuenta.TipoCuenta = i.TipoDeCuenta;
                        cuenta.Nivel = i.Nivel;
                        cuenta.IdUsuario = i.IdUsuario;
                        cuenta.IdEmpresa = i.IdEmpresa;
                        cuenta.IdCuentaPadre = i.IdCuentaPadre;
                        // cuenta.children = CuentaHijosAux(cuenta.idCuenta, idempresa);
                        cuentas.Add(cuenta);
                    }

                    //  empresas = empresa;

                    return cuentas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de empresas");
                }



            }
        }

        */

        public Categoria ModificarCategoria(int idcategoria, string nombre, string descripcion, int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    if (string.IsNullOrEmpty(nombre))
                    {
                        throw new MensageException("Ingrese un nombre");
                    }

                    var validar = (from x in esquema.Categoria
                                   where x.IdEmpresa == idempresa
                                   && x.IdCategoria != idcategoria
                                   && x.Nombre.Trim().ToUpper() == nombre.Trim().ToUpper()
                                   select x
                                   ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new MensageException("El nombre de la Categoria en esta empresa ya existe");
                    }

                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdCategoria == idcategoria
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("Error no se puedo obtener la Categoria");
                    }

                    linqcuenta.Nombre = nombre;
                    linqcuenta.Descripcion = descripcion;
                    esquema.SaveChanges();

                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException(ex.Message);
                }

            }
        }

        public List<ECategoria> CategoriaHijos(int idPadre, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdEmpresa == idempresa
                                      && x.IdCategoriaPadre == idPadre
                                      select x).ToList();


                    List<ECategoria> categorias = new List<ECategoria>();


                    //  empresas = empresa;


                    foreach (var i in linqcuenta)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.id = i.IdCategoria;
                        categoria.idCategoria = i.IdCategoria;
                        categoria.Nombre = i.Nombre;
                        categoria.Descripcion = i.Descripcion;
                        categoria.text = i.Nombre;
                        categoria.IdUsuario = i.IdUsuario;
                        categoria.IdEmpresa = i.IdEmpresa;
                        categoria.IdCategoriaPadre = i.IdCategoriaPadre;
                        categoria.children = CategoriaHijos(categoria.idCategoria, idempresa);
                        categorias.Add(categoria);
                    }

                    return categorias;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de categorias");
                }



            }
        }


        public List<ECategoria> CategoriaHijosAux(int idPadre, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Categoria
                                      where x.IdEmpresa == idempresa
                                      && x.IdCategoriaPadre == idPadre
                                      select x).ToList();


                    List<ECategoria> categorias = new List<ECategoria>();


                    //  empresas = empresa;


                    foreach (var i in linqcuenta)
                    {
                        ECategoria categoria = new ECategoria();
                        categoria.id = i.IdCategoria;
                        categoria.idCategoria = i.IdCategoria;
                        categoria.Nombre = i.Nombre;
                        categoria.Descripcion = i.Descripcion;
                        categoria.text = i.Nombre;
                        categoria.IdUsuario = i.IdUsuario;
                        categoria.IdEmpresa = i.IdEmpresa;
                        categoria.IdCategoriaPadre = i.IdCategoriaPadre;
                        categoria.children = CategoriaHijos(categoria.idCategoria, idempresa);
                        categorias.Add(categoria);
                    }

                    return categorias;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de Categorias");
                }



            }
        }

        public Categoria Registro(Categoria Entidad, int idcategoria, int padre)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (string.IsNullOrEmpty(Entidad.Nombre))
                    {
                        throw new MensageException("Ingrese un nombre");
                    }

                    var validar = (from x in esquema.Categoria
                                   where x.IdEmpresa == Entidad.IdEmpresa
                                   && x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                   select x
                                   ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new MensageException("Ya existe este nombre de categoria");
                    }

                    if (padre == 0)
                    {
                        //hijos
                        var cuentapadre = (from x in esquema.Categoria
                                           where x.IdEmpresa == Entidad.IdEmpresa
                                           && x.IdCategoria == idcategoria
                                           select x
                                          ).FirstOrDefault();

                        if (cuentapadre != null)
                        {

                            esquema.Categoria.Add(Entidad);
                            esquema.SaveChanges();


                        }
                        else
                        {
                            throw new MensageException("Error al registrar la categoria");
                        }


                    }
                    else if (padre == 1)
                    {
                        //padres

                        var linqpadres = (from x in esquema.Categoria
                                          where x.IdEmpresa == Entidad.IdEmpresa
                                          select x
                                         ).FirstOrDefault();


                        esquema.Categoria.Add(Entidad);
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

        /*public List<ERDatosBasicoCuenta> ReporteDatosBasicoCuenta(string usuario, string empresa)
        {
            try
            {


                List<ERDatosBasicoCuenta> basicos = new List<ERDatosBasicoCuenta>();
                ERDatosBasicoCuenta eRDatosBasico = new ERDatosBasicoCuenta();
                eRDatosBasico.Usuario = usuario;
                //   eRDatosBasico.NombreGestion = gestion;
                eRDatosBasico.NombreEmpresa = empresa;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }*/
        public void Duplicados(int? Id, List<EAuxiliar> auxiliar)
        {

            int contador = 0;

            foreach (var i in auxiliar)
            {
                if (i.Id == Id)
                {
                    contador = contador + 1;
                }
            }

            if (contador > 1)
            {
                throw new MensageException("Existen Categorias repetidas en la configuracion");
            }

        }


    }
}
