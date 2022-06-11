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
    public class LGestion : LLogica<Gestion>
    {

        public List<Gestion> listarGestion(int idempresa, int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var gestion = (from x in esquema.Gestion
                                   where x.IdEmpresa == idempresa
                                   orderby x.Fechainicio
                                   select x).ToList();

                    List<Gestion> gestiones = new List<Gestion>();

                    gestiones = gestion;

                    return gestiones;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de gestiones");
                }

            }
        }


        public Gestion Registro(Gestion Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);


                    var listagestion = (from x in esquema.Gestion
                                        where x.Estado == (int)EstadoGestion.Abierto
                                         && x.IdEmpresa == Entidad.IdEmpresa
                                        select x).ToList();

                    if (listagestion.Count() > 1)
                    {
                        throw new MensageException("Ya Existen 2 Gestiones Abiertas");
                    }

                    Gestion gestionexiste = (from x in esquema.Gestion
                                             where x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || (Entidad.Fechainicio >= x.Fechainicio && Entidad.Fechainicio <= x.Fechafin)
                                             || (Entidad.Fechafin >= x.Fechainicio && Entidad.Fechafin <= x.Fechafin)
                                             || (x.Fechainicio >= Entidad.Fechainicio && x.Fechainicio <= Entidad.Fechafin)
                                             || (x.Fechafin >= Entidad.Fechainicio && x.Fechafin <= Entidad.Fechafin))
                                             select x).FirstOrDefault();

                    validarExistencia(gestionexiste, Entidad);


                    esquema.Gestion.Add(Entidad);
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


        public void ValidarGestionesAbiertas(int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {



                    var listagestion = (from x in esquema.Gestion
                                        where x.Estado == (int)EstadoGestion.Abierto
                                         && x.IdEmpresa == idempresa
                                        select x).ToList();

                    if (listagestion.Count() > 1)
                    {
                        throw new MensageException("Ya existen 2 gestiones abiertas");
                    }



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


        public Gestion Editar(Gestion Entidad)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad);


                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == Entidad.IdGestion
                                   select x).FirstOrDefault();


                    if (gestion == null)
                    {
                        throw new MensageException("No se puedo obtener la gestion");

                    }

                    if (gestion.Estado == (int)EstadoGestion.Cerrado)
                    {
                        throw new MensageException("No se puede modificar una gestion cerrada");
                    }

                    var listagestion = (from x in esquema.Gestion
                                        where x.IdGestion != Entidad.IdGestion
                                         && x.Estado == (int)EstadoGestion.Abierto
                                         && x.IdEmpresa == Entidad.IdEmpresa
                                        select x).ToList();

                    if (listagestion.Count() > 1)
                    {
                        throw new MensageException("Ya existen 2 gestiones abiertas");
                    }

                    Gestion gestionexiste = (from x in esquema.Gestion
                                             where x.IdGestion != Entidad.IdGestion
                                             && x.IdEmpresa == Entidad.IdEmpresa
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || (Entidad.Fechainicio >= x.Fechainicio && Entidad.Fechainicio <= x.Fechafin)
                                             || (Entidad.Fechafin >= x.Fechainicio && Entidad.Fechafin <= x.Fechafin)
                                             || (x.Fechainicio >= Entidad.Fechainicio && x.Fechainicio <= Entidad.Fechafin)
                                             || (x.Fechafin >= Entidad.Fechainicio && x.Fechafin <= Entidad.Fechafin))
                                             select x).FirstOrDefault();

                    validarExistencia(gestionexiste, Entidad);



                    if (gestion.Fechainicio == Entidad.Fechainicio && gestion.Fechafin == Entidad.Fechafin)
                    {
                        gestion.Nombre = Entidad.Nombre;
                        gestion.IdUsuario = Entidad.IdUsuario;
                        esquema.SaveChanges();
                    }
                    else
                    {
                        if (gestion.Periodo.Count() > 0)
                        {
                            throw new MensageException("No se puede eliminar la gestion porque tiene registrado un periodo");

                        }
                        else
                        {
                            gestion.Nombre = Entidad.Nombre;
                            gestion.Fechainicio = Entidad.Fechainicio;
                            gestion.Fechafin = Entidad.Fechafin;
                            gestion.IdUsuario = Entidad.IdUsuario;
                            esquema.SaveChanges();
                        }
                    }


                    // esquema.Gestion.Add(Entidad);
                    //  esquema.SaveChanges();

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


        public void Eliminar(int idgestion)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();

                    if (gestion == null)
                    {
                        throw new MensageException("No se puede obtener la gestion");
                    }

                    if (gestion.Estado == (int)EstadoGestion.Cerrado)
                    {
                        throw new MensageException("No se puede eliminar una gestion cerrada");
                    }

                    if (gestion.Periodo.Count() > 0)
                    {
                        throw new MensageException("No se puede eliminar la gestion porque tiene registrado un periodo");

                    }
                    else
                    {
                        esquema.Gestion.Remove(gestion);
                        esquema.SaveChanges();
                    }

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


        public Gestion obtenerGestion(int idgestion)
        {

            using (var esquema = GetEsquema())
            {

                try
                {


                    var gestion = (from x in esquema.Gestion
                                   where x.IdGestion == idgestion
                                   select x).FirstOrDefault();


                    if (gestion == null)
                    {
                        throw new MensageException("No se puedo obtener la gestion");
                    }

                    return gestion;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la gestion");
                }


            }
        }
        public List<ERGestion> ReporteGestion(int idempresa)
        {
            List<ERGestion> gestiones = new List<ERGestion>();

            try
            {
                using (var esquema = GetEsquema())
                {
                    var gestion = (from x in esquema.Gestion
                                   where x.IdEmpresa == idempresa orderby x.Fechainicio
                                   select x).ToList();



                    foreach (var i in gestion)
                    {
                        ERGestion g = new ERGestion();
                        g.NombreGestion = i.Nombre;
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

                        gestiones.Add(g);
                    }
                }


            }
            catch (Exception ex)
            {
                throw new MensageException("Error no se puedo obtener el reporte de gestiones");
            }



            return gestiones;
        }
        public List<ERDatosBasicoGestion> ReporteDatosBasicoGestion(string usuario, string empresa)
        {
            try
            {


                List<ERDatosBasicoGestion> basicos = new List<ERDatosBasicoGestion>();
                ERDatosBasicoGestion eRDatosBasico = new ERDatosBasicoGestion();
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


        public void Validacion(Gestion Entidad)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("El nombre es obligatorio.");
            }

            if (Entidad.Fechainicio > Entidad.Fechafin)
            {
                throw new MensageException("La fecha de inicio debe ser menor a la fecha fin.");
            }

        }


        public void validarExistencia(Gestion Existe, Gestion Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya existe una gestion con este nombre.");
                }

                if (Existe.Fechainicio == Entidad.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Existe.Fechafin == Entidad.Fechainicio)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Existe.Fechainicio == Entidad.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Existe.Fechafin == Entidad.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Entidad.Fechainicio >= Existe.Fechainicio && Entidad.Fechainicio <= Existe.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Entidad.Fechafin >= Existe.Fechainicio && Entidad.Fechafin <= Existe.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Existe.Fechainicio >= Entidad.Fechainicio && Existe.Fechainicio <= Entidad.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

                if (Existe.Fechafin >= Entidad.Fechainicio && Existe.Fechafin <= Entidad.Fechafin)
                {
                    throw new MensageException("Hay solapamiento con otra gestion.");
                }

            }
        }


    }
}
