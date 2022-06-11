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
    public class LPeriodo : LLogica<Periodo>
    {
        public List<Periodo> listarPeriodo(int idgestion)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var periodo = (from x in esquema.Periodo
                                   where x.IdGestion == idgestion orderby x.Fechainicio
                                   select x).ToList();

                    List<Periodo> periodos = new List<Periodo>();

                    periodos = periodo;

                    return periodos;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de periodos");
                }

            }
        }

        public Periodo obtenerPeriodo(int idperiodo)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var periodo = (from x in esquema.Periodo
                                   where x.IdPeriodo == idperiodo
                                   select x).FirstOrDefault();



                    return periodo;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de periodos");
                }

            }
        }



        public Periodo Registro(Periodo Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);


                    var gestion = LGestion.Logica.LGestion.obtenerGestion(Entidad.IdGestion);

                    if (gestion == null)
                    {
                        throw new MensageException("No se pudo encontrar la gestion");
                    }

                    if (!(Entidad.Fechainicio >= gestion.Fechainicio && Entidad.Fechainicio <= gestion.Fechafin))
                    {
                        throw new MensageException("Fuera del rango de fechas de la " + gestion.Nombre);
                    }

                    if (!(Entidad.Fechafin >= gestion.Fechainicio && Entidad.Fechafin <= gestion.Fechafin))
                    {
                        throw new MensageException("Fuera del rango de fechas de la " + gestion.Nombre);
                    }


                    /*  var listaperiodo = (from x in esquema.Periodo
                                          where x.Estado == (int)EstadoPeriodo.Abierto
                                           && x.IdGestion == Entidad.IdGestion
                                          select x).ToList();

                     if (listaperiodo.Count() > 1)
                      {
                          throw new MensageException("Ya existe 2 periodos abiertos, no es posible registrar otra periodo");
                      }*/

                    Periodo periodoexiste = (from x in esquema.Periodo
                                             where x.IdGestion == Entidad.IdGestion
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || (Entidad.Fechainicio >= x.Fechainicio && Entidad.Fechainicio <= x.Fechafin)
                                             || (Entidad.Fechafin >= x.Fechainicio && Entidad.Fechafin <= x.Fechafin)
                                             || (x.Fechainicio >= Entidad.Fechainicio && x.Fechainicio <= Entidad.Fechafin)
                                             || (x.Fechafin >= Entidad.Fechainicio && x.Fechafin <= Entidad.Fechafin))
                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);


                    esquema.Periodo.Add(Entidad);
                    esquema.SaveChanges();

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


        public Periodo Editar(Periodo Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);

                    var gestion = LGestion.Logica.LGestion.obtenerGestion(Entidad.IdGestion);

                    if (gestion == null)
                    {
                        throw new MensageException("No se pudo encontrar la gestion");
                    }

                    if (!(Entidad.Fechainicio >= gestion.Fechainicio && Entidad.Fechainicio <= gestion.Fechafin))
                    {
                        throw new MensageException("Fuera del rango de fechas de la " + gestion.Nombre);
                    }

                    if (!(Entidad.Fechafin >= gestion.Fechainicio && Entidad.Fechafin <= gestion.Fechafin))
                    {
                        throw new MensageException("Fuera del rango de fechas de la " + gestion.Nombre);
                    }


                    var periodo = (from x in esquema.Periodo
                                   where x.IdPeriodo == Entidad.IdPeriodo
                                   select x).FirstOrDefault();


                    if (periodo == null)
                    {
                        throw new MensageException("No se puedo obtener el periodo");

                    }

                    if (periodo.Estado == (int)EstadoGestion.Cerrado)
                    {
                        throw new MensageException("No se puede modificar un periodo cerrado");
                    }

                    /* var listaperiodo = (from x in esquema.Periodo
                                         where x.IdGestion != Entidad.IdGestion
                                          && x.Estado == (int)EstadoGestion.Abierto
                                          && x.IdGestion == Entidad.IdGestion
                                         select x).ToList();

                     if (listaperiodo.Count() > 1)
                     {
                         throw new MensageException("Ya existe 2 periodos abiertos, no es posible registrar otra periodo");
                     }*/

                    Periodo periodoexiste = (from x in esquema.Periodo
                                             where x.IdPeriodo != Entidad.IdPeriodo
                                             && x.IdGestion == Entidad.IdGestion
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || (Entidad.Fechainicio >= x.Fechainicio && Entidad.Fechainicio <= x.Fechafin)
                                             || (Entidad.Fechafin >= x.Fechainicio && Entidad.Fechafin <= x.Fechafin)
                                             || (x.Fechainicio >= Entidad.Fechainicio && x.Fechainicio <= Entidad.Fechafin)
                                             || (x.Fechafin >= Entidad.Fechainicio && x.Fechafin <= Entidad.Fechafin))

                                             select x).FirstOrDefault();

                    validarExistencia(periodoexiste, Entidad);



                    if (periodo.Fechainicio == Entidad.Fechainicio && periodo.Fechafin == Entidad.Fechafin)
                    {
                        periodo.Nombre = Entidad.Nombre;
                        periodo.IdUsuario = Entidad.IdUsuario;
                        esquema.SaveChanges();
                    }
                    else
                    {
                        /*if (periodo.Periodo.Count() > 0)
                        {
                            throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                        }
                        else
                        {*/
                        periodo.Nombre = Entidad.Nombre;
                        periodo.Fechainicio = Entidad.Fechainicio;
                        periodo.Fechafin = Entidad.Fechafin;
                        periodo.IdUsuario = Entidad.IdUsuario;
                        esquema.SaveChanges();
                        // }
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

        public void Eliminar(int idperiodo)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var periodo = (from x in esquema.Periodo
                                   where x.IdPeriodo == idperiodo
                                   select x).FirstOrDefault();

                    if (periodo == null)
                    {
                        throw new MensageException("No se puede obtener el periodo");
                    }

                    if (periodo.Estado == (int)EstadoPeriodo.Cerrado)
                    {
                        throw new MensageException("No se puede eliminar un periodo cerrado");
                    }

                    /* if (gestion.Periodo.Count() > 0)
                     {
                         throw new MensageException("No se puede eliminar la gestion, tiene registrado un periodo");

                     }
                     else
                     {*/
                    esquema.Periodo.Remove(periodo);
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


        


        public List<ERDatosBasicoPeriodo> ReporteDatosBasicoPeriodo(string usuario, string empresa, string gestion)
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


        public void Validacion(Periodo Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("El nombre es obligatorio.");
            }

            if (Entidad.Fechainicio > Entidad.Fechafin)
            {
                throw new MensageException("La fecha inicio debe ser menor a la fecha fin.");
            }

        }


        public void validarExistencia(Periodo Existe, Periodo Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya existe el nombre del periodo en esta gestion.");
                }

                if (Existe.Fechainicio == Entidad.Fechainicio)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Existe.Fechafin == Entidad.Fechainicio)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Existe.Fechainicio == Entidad.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Existe.Fechafin == Entidad.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Entidad.Fechainicio >= Existe.Fechainicio && Entidad.Fechainicio <= Existe.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Entidad.Fechafin >= Existe.Fechainicio && Entidad.Fechafin <= Existe.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }


                if (Existe.Fechainicio >= Entidad.Fechainicio && Existe.Fechainicio <= Entidad.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

                if (Existe.Fechafin >= Entidad.Fechainicio && Existe.Fechafin <= Entidad.Fechafin)
                {
                    throw new MensageException("Existe solapamiento con otro perido.");
                }

            }
        }

        public List<ERPeriodo> ReportePeriodo(int idgestion)
        {
            List<ERPeriodo> periodos = new List<ERPeriodo>();

            try
            {
                using (var esquema = GetEsquema())
                {
                    var periodo = (from x in esquema.Periodo
                                   where x.IdGestion == idgestion orderby x.Fechainicio
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
    }
}
