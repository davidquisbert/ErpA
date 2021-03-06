using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidad;
using Entidad.Estados;

namespace Logica
{
    public class LEmpresa : LLogica<Empresa>
    {

        public List<EREmpresa> ReporteListaEmpresaReportView()
        {
            List<EREmpresa> empresas = new List<EREmpresa>();

            try
            {
                using (var esquema = GetEsquema())
                {
                    var empresa = (from x in esquema.Empresa
                                       // where x.IdUsuario == idusuario
                                   select x).ToList();



                    foreach (var i in empresa)
                    {
                        EREmpresa e = new EREmpresa();

                        e.Sigla = i.Sigla;
                        e.Nombre = i.Nombre;
                        e.NIT = i.Nit;
                        e.Telefono = i.Telefono;
                        e.Correo = i.Correo;

                        empresas.Add(e);

                    }
                }


            }
            catch (Exception ex)
            {
                // throw new MensageException("Error no se puedo obtener la lista de empresas");
            }



            return empresas;
        }
        public List<Empresa> listarEmpresa(int idusuario)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var empresa = (from x in esquema.Empresa select x).ToList();

                    List<Empresa> empresas = new List<Empresa>();

                    empresas = empresa;

                    return empresas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de empresas");
                }



            }
        }



        public Empresa obtenerEmpresa(int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {

                    if (idempresa < 0)
                    {
                        throw new MensageException("Seleccione una empresa");
                    }

                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();


                    if (empresa == null)
                    {
                        throw new MensageException("No se puede obtener la empresa");
                    }

                    return empresa;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la empresas");
                }


            }
        }

        

        

        public void Eliminar(int idempresa)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();

                    if (empresa == null)
                    {
                        throw new MensageException("No se puede obtener la empresa");
                    }

                    if (empresa.Gestion.Count() > 0)
                    {
                        throw new MensageException("No se puede eliminar la empresa por que ya tiene registada una gestion");

                    }
                    else
                    {
                        esquema.Empresa.Remove(empresa);
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

        

        public void validarExistencia(Empresa Existe, Empresa Entidad)
        {
            if (Existe != null)
            {

                if (Existe.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper())
                {
                    throw new MensageException("Ya existe otra Empresa Con este Nombre.");
                }

                if (Existe.Nit.Trim().ToUpper() == Entidad.Nit.Trim().ToUpper())
                {
                    throw new MensageException("Ya existe otra Empresa Con este Nit.");
                }

                if (Existe.Sigla.Trim().ToUpper() == Entidad.Sigla.Trim().ToUpper())
                {
                    throw new MensageException("Ya existe otra Empresa Con esta Sigla.");
                }

            }
        }


        public List<ERDatosBasico> ReporteDatosBasico(string usuario)
        {
            try
            {


                List<ERDatosBasico> basicos = new List<ERDatosBasico>();
                ERDatosBasico eRDatosBasico = new ERDatosBasico();
                eRDatosBasico.Usuario = usuario;
                eRDatosBasico.FechaActual = DateTime.Now.ToString("dd/MM/yyyy");

                basicos.Add(eRDatosBasico);

                return basicos;
            }
            catch (Exception ex)
            {
                throw new MensageException("Ha ocurrido un error.");
            }
        }

        public Empresa Registro(Empresa Entidad, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    Validacion(Entidad,idmoneda);

                    Empresa empresaexiste = (from x in esquema.Empresa
                                             where x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || x.Nit.Trim().ToUpper() == Entidad.Nit.Trim().ToUpper()
                                             || x.Sigla.Trim().ToUpper() == Entidad.Sigla.Trim().ToUpper()
                                             select x).FirstOrDefault();

                    validarExistencia(empresaexiste, Entidad);

                    esquema.Empresa.Add(Entidad);
                    esquema.SaveChanges();

                    EmpresaMoneda empresaMoneda = new EmpresaMoneda()
                    {
                        Activo = (int)EstadoMonedaEmpresa.Activo,
                        FechaRegistro = DateTime.Now,
                        IdEmpresa = Entidad.IdEmpresa,
                        IdMonedaPrincipal = idmoneda,
                        IdUsuario = Entidad.IdUsuario

                    };

                    esquema.EmpresaMoneda.Add(empresaMoneda);
                    esquema.SaveChanges();


                    for (int i = 0; i < 5; i++)
                    {

                        switch (i)
                        {
                            case 0:
                                Cuenta cuenta = new Cuenta()
                                {
                                    Nombre = "Activo",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,



                                };
                                cuenta = LCuenta.Logica.LCuenta.Registro(cuenta, 0, 1);
                                break;
                            case 1:
                                Cuenta cuenta1 = new Cuenta()
                                {
                                    Nombre = "Pasivo",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,



                                };
                                cuenta1 = LCuenta.Logica.LCuenta.Registro(cuenta1, 0, 1);
                                break;
                            case 2:
                                Cuenta cuenta2 = new Cuenta()
                                {
                                    Nombre = "Patrimonio",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,



                                };
                                cuenta2 = LCuenta.Logica.LCuenta.Registro(cuenta2, 0, 1);
                                break;
                            case 3:
                                Cuenta cuenta3 = new Cuenta()
                                {
                                    Nombre = "Ingresos",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,



                                };
                                cuenta3 = LCuenta.Logica.LCuenta.Registro(cuenta3, 0, 1);
                                break;
                            case 4:
                                Cuenta cuenta4 = new Cuenta()
                                {
                                    Nombre = "Egresos",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,



                                };
                                cuenta4 = LCuenta.Logica.LCuenta.Registro(cuenta4, 0, 1);

                                Cuenta cuentahijo1 = new Cuenta()
                                {
                                    Nombre = "Costos",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,
                                    IdCuentaPadre = cuenta4.IdCuenta


                                };
                                cuentahijo1 = LCuenta.Logica.LCuenta.Registro(cuentahijo1, cuenta4.IdCuenta, 0);

                                Cuenta cuentahijo2 = new Cuenta()
                                {
                                    Nombre = "Gastos",
                                    Codigo = "",
                                    Nivel = 0,
                                    TipoDeCuenta = (int)TipoCuenta.Global,
                                    IdUsuario = Entidad.IdUsuario,
                                    IdEmpresa = Entidad.IdEmpresa,
                                    IdCuentaPadre = cuenta4.IdCuenta


                                };
                                cuentahijo2 = LCuenta.Logica.LCuenta.Registro(cuentahijo2, cuenta4.IdCuenta, 0);
                                break;
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
        public Empresa Editar(Empresa Entidad, int idmoneda)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    Validacion(Entidad,idmoneda);

                    Empresa empresaexiste = (from x in esquema.Empresa
                                             where x.IdEmpresa != Entidad.IdEmpresa
                                             && (x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                             || x.Nit.Trim().ToUpper() == Entidad.Nit.Trim().ToUpper()
                                             || x.Sigla.Trim().ToUpper() == Entidad.Sigla.Trim().ToUpper())
                                             select x).FirstOrDefault();

                    validarExistencia(empresaexiste, Entidad);


                    Empresa empresa = (from x in esquema.Empresa
                                       where x.IdEmpresa == Entidad.IdEmpresa
                                       select x).FirstOrDefault();
                    
                    if (empresa == null)
                    {
                        throw new MensageException("No se puede obtener la empresa");
                    }

                    empresa.Nombre = Entidad.Nombre;
                    empresa.Nit = Entidad.Nit;
                    empresa.Sigla = Entidad.Sigla;
                    empresa.Telefono = Entidad.Telefono;
                    empresa.Correo = Entidad.Correo;
                    empresa.Niveles = Entidad.Niveles;
                    empresa.Direccion = Entidad.Direccion;
                    // empresa.IdUsuario = Entidad.IdUsuario;


                    var moneda = LMoneda.Logica.LMoneda.obtenerPrimerMonedadXempresa(empresa.IdEmpresa);
                    var EMPmoneda = LMoneda.Logica.LMoneda.obtenerEmpresaMonedavalidacion(empresa.IdEmpresa);
                    var monedaActiva = LMoneda.Logica.LMoneda.obtenerEmpresaMonedaActiva(empresa.IdEmpresa);
                    if (moneda != null)
                    {
                        if (idmoneda != moneda.IdMonedaPrincipal)
                        {
                            throw new MensageException("No se puede modificar la moneda principal, Ya existe una moneda alternativa");
                        }
                    }
                    else
                    {
                        EditarMPrincipal(Entidad.IdEmpresa,idmoneda);
                    }

                    esquema.SaveChanges();
                    
                    return empresa;
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
        public EmpresaMoneda EditarMPrincipal(int idempresa,int idmoneda)
        {
            using (var esquema = GetEsquema())
            {
                try
                {

                    var monedaActiva = (from x in esquema.EmpresaMoneda
                                        where x.IdEmpresa == idempresa
                                        && x.Activo == (int)EstadoMonedaEmpresa.Activo
                                        select x).FirstOrDefault();

                    monedaActiva.FechaRegistro = DateTime.Now;
                    monedaActiva.IdMonedaPrincipal = idmoneda;
                   
                    esquema.SaveChanges();

                    return monedaActiva;

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
        public void Validacion(Empresa Entidad, int idMoneda)
        {

            if (string.IsNullOrEmpty(Entidad.Nombre))
            {
                throw new MensageException("El nombre es obligatorio.");
            }

            if (string.IsNullOrEmpty(Entidad.Sigla))
            {
                throw new MensageException("La sigla es obligatorio.");
            }
            if (!string.IsNullOrEmpty(Entidad.Correo))
            {
                try
                {
                    new MailAddress(Entidad.Correo);
                }
                catch (Exception ex)
                {
                    throw new MensageException("Ingrese un correo valido.");
                }
            }
            if (string.IsNullOrEmpty(Entidad.Nit))
            {
                throw new MensageException("El nit es obligatorio.");
            }

            if (idMoneda == -1)
            {
                throw new MensageException("Seleccione una moneda.");
            }
            /*  if (string.IsNullOrEmpty(Entidad.Direccion))
              {
                  throw new MensageException("La direccion es obligatorio.");
              }*/
            /*  if (Entidad.Niveles<3 || Entidad.Niveles>8  )
              {
                  throw new MensageException("El nivel tiene que ser entre 3 o 7.");
              }
              if (Entidad.Niveles < 3 || Entidad.Niveles > 8)
              {
                  throw new MensageException("El nivel tiene que ser entre 3 o 7.");
              }*/


        }



    }
}
