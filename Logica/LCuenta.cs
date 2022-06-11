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
    public class LCuenta : LLogica<Cuenta>
    {

        public List<ECuenta> listarCuentas(int idusuario, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdEmpresa == idempresa
                                      && x.IdCuentaPadre == null
                                      select x).ToList();

                    List<ECuenta> cuentas = new List<ECuenta>();


                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.id = i.IdCuenta;
                        cuenta.idCuenta = i.IdCuenta;
                        cuenta.Codigo = i.Codigo;
                        cuenta.Nombre = i.Nombre;
                        cuenta.text = i.Codigo + " " + i.Nombre;
                        cuenta.TipoCuenta = i.TipoDeCuenta;
                        cuenta.Nivel = i.Nivel;
                        cuenta.IdUsuario = i.IdUsuario;
                        cuenta.IdEmpresa = i.IdEmpresa;
                        cuenta.IdCuentaPadre = i.IdCuentaPadre;
                        cuenta.children = CuentaHijos(cuenta.idCuenta, idempresa);
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


        public Cuenta obtenerCuenta(int idcuenta)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdCuenta == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("No se obtuvo la cuenta");
                    }

                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException("No se obtuvo la cuenta");
                }

            }
        }

        public List<Cuenta> listarCuentaDetalle(int idempresa)
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
        }


        public Cuenta EliminarCuenta(int idcuenta)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdCuenta == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("Error no se puedo obtener la cuenta");
                    }


                    if (linqcuenta.Cuenta1.Count() > 0)
                    {
                        throw new MensageException("No se puede eliminar la cuenta de esta empresa esta relacionada");

                    }

                    /* if(linqcuenta.IdCuentaPadre != null)
                     {
                         throw new MensageException("No se puede eliminar la cuenta de esta empresa esta relacionada");
                     }*/


                    esquema.Cuenta.Remove(linqcuenta);
                    esquema.SaveChanges();


                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException(ex.Message);
                }

            }
        }


        public List<ECuenta> listarCuentasReporte(int idusuario, int idempresa)
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
                        if (i.Nivel==1)
                        {
                            cuenta.text = ""+ i.Codigo + " " + i.Nombre;
                        }else if(i.Nivel == 2)
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
        public Empresa RegistroIntegracion(Empresa integracionlist, int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {

                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x
                                   ).FirstOrDefault();
                    if (empresa == null)
                    {
                        throw new MensageException("Error no se puedo obtener la Empresa");
                    }
                    if(integracionlist.IdCuentaCaja==integracionlist.IdCuentaCreditoFiscal||
                       integracionlist.IdCuentaCaja == integracionlist.IdCuentaDebitoFiscal ||
                       integracionlist.IdCuentaCaja == integracionlist.IdCuentaCompras ||
                       integracionlist.IdCuentaCaja == integracionlist.IdCuentaVentas ||
                       integracionlist.IdCuentaCaja == integracionlist.IdCuentaIt ||
                       integracionlist.IdCuentaCaja == integracionlist.IdCuentaItPorPagar ||
                       integracionlist.IdCuentaCreditoFiscal == integracionlist.IdCuentaDebitoFiscal ||
                       integracionlist.IdCuentaCreditoFiscal == integracionlist.IdCuentaCompras ||
                       integracionlist.IdCuentaCreditoFiscal == integracionlist.IdCuentaVentas ||
                       integracionlist.IdCuentaCreditoFiscal == integracionlist.IdCuentaIt ||
                       integracionlist.IdCuentaCreditoFiscal == integracionlist.IdCuentaItPorPagar ||
                       integracionlist.IdCuentaDebitoFiscal == integracionlist.IdCuentaCompras ||
                       integracionlist.IdCuentaDebitoFiscal == integracionlist.IdCuentaVentas ||
                       integracionlist.IdCuentaDebitoFiscal == integracionlist.IdCuentaIt ||
                       integracionlist.IdCuentaDebitoFiscal == integracionlist.IdCuentaItPorPagar ||
                       integracionlist.IdCuentaCompras == integracionlist.IdCuentaVentas ||
                       integracionlist.IdCuentaCompras == integracionlist.IdCuentaIt ||
                       integracionlist.IdCuentaCompras == integracionlist.IdCuentaItPorPagar ||
                       integracionlist.IdCuentaVentas == integracionlist.IdCuentaIt ||
                       integracionlist.IdCuentaVentas == integracionlist.IdCuentaItPorPagar ||
                       integracionlist.IdCuentaIt == integracionlist.IdCuentaItPorPagar
                        )
                    {
                        throw new Exception("Cuentas Duplicadas");
                    }
                    else
                    {
                        empresa.Integracion = integracionlist.Integracion;
                        empresa.IdCuentaCaja = integracionlist.IdCuentaCaja;
                        empresa.IdCuentaCreditoFiscal = integracionlist.IdCuentaCreditoFiscal;
                        empresa.IdCuentaDebitoFiscal = integracionlist.IdCuentaDebitoFiscal;
                        empresa.IdCuentaCompras = integracionlist.IdCuentaCompras;
                        empresa.IdCuentaVentas = integracionlist.IdCuentaVentas;
                        empresa.IdCuentaIt = integracionlist.IdCuentaIt;
                        empresa.IdCuentaItPorPagar = integracionlist.IdCuentaItPorPagar;
                        esquema.SaveChanges();

                        return empresa;
                    }
                }
                catch (Exception ex)
                {
                    throw new MensageException(ex.Message);
                }

            }
        }
        public Cuenta ModificarCuenta(int idcuenta, string nombre, int idempresa)
        {

            using (var esquema = GetEsquema())
            {

                try
                {
                    if (string.IsNullOrEmpty(nombre))
                    {
                        throw new MensageException("Ingrese un nombre");
                    }

                    var validar = (from x in esquema.Cuenta
                                   where x.IdEmpresa == idempresa
                                   && x.IdCuenta != idcuenta
                                   && x.Nombre.Trim().ToUpper() == nombre.Trim().ToUpper()
                                   select x
                                   ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new MensageException("El nombre de la cuenta en esta empresa ya existe");
                    }

                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdCuenta == idcuenta
                                      select x).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        throw new MensageException("Error no se puedo obtener la cuenta");
                    }

                    linqcuenta.Nombre = nombre;
                    esquema.SaveChanges();

                    return linqcuenta;

                }
                catch (Exception ex)
                {
                    throw new MensageException(ex.Message);
                }

            }
        }
        public Empresa obtenerEmpresa(int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == idempresa
                                   select x).FirstOrDefault();


                    return empresa;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la Empresa");
                }



            }
        }
        public List<ECuenta> CuentaHijos(int idPadre, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdEmpresa == idempresa
                                      && x.IdCuentaPadre == idPadre
                                      select x).ToList();


                    List<ECuenta> cuentas = new List<ECuenta>();


                    //  empresas = empresa;


                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.id = i.IdCuenta;
                        cuenta.idCuenta = i.IdCuenta;
                        cuenta.Codigo = i.Codigo;
                        cuenta.Nombre = i.Nombre;
                        cuenta.text = i.Codigo + " " + i.Nombre;
                        cuenta.TipoCuenta = i.TipoDeCuenta;
                        cuenta.Nivel = i.Nivel;
                        cuenta.IdUsuario = i.IdUsuario;
                        cuenta.IdEmpresa = i.IdEmpresa;
                        cuenta.IdCuentaPadre = i.IdCuentaPadre;
                        cuenta.children = CuentaHijos(cuenta.idCuenta, idempresa);
                        cuentas.Add(cuenta);
                    }

                    return cuentas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de empresas");
                }



            }
        }


        public List<ECuenta> CuentaHijosAux(int idPadre, int idempresa)
        {
            using (var esquema = GetEsquema())
            {

                try
                {
                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdEmpresa == idempresa
                                      && x.IdCuentaPadre == idPadre
                                      select x).ToList();


                    List<ECuenta> cuentas = new List<ECuenta>();


                    //  empresas = empresa;


                    foreach (var i in linqcuenta)
                    {
                        ECuenta cuenta = new ECuenta();
                        cuenta.id = i.IdCuenta;
                        cuenta.idCuenta = i.IdCuenta;
                        cuenta.Codigo = i.Codigo;
                        cuenta.Nombre = i.Nombre;
                        cuenta.text = " " + i.Codigo + " " + i.Nombre;
                        cuenta.TipoCuenta = i.TipoDeCuenta;
                        cuenta.Nivel = i.Nivel;
                        cuenta.IdUsuario = i.IdUsuario;
                        cuenta.IdEmpresa = i.IdEmpresa;
                        cuenta.IdCuentaPadre = i.IdCuentaPadre;
                        cuenta.children = CuentaHijos(cuenta.idCuenta, idempresa);
                        cuentas.Add(cuenta);
                    }

                    return cuentas;

                }
                catch (Exception ex)
                {
                    throw new MensageException("Error no se puedo obtener la lista de empresas");
                }



            }
        }

        public Cuenta Registro(Cuenta Entidad, int idcuenta, int padre)
        {
            using (var esquema = GetEsquema())
            {
                try
                {
                    if (string.IsNullOrEmpty(Entidad.Nombre))
                    {
                        throw new MensageException("Ingrese un nombre");
                    }
                    var empresa = (from x in esquema.Empresa
                                   where x.IdEmpresa == Entidad.IdEmpresa
                                   select x).FirstOrDefault();

                    var validar = (from x in esquema.Cuenta
                                   where x.IdEmpresa == Entidad.IdEmpresa
                                   && x.Nombre.Trim().ToUpper() == Entidad.Nombre.Trim().ToUpper()
                                   select x
                                    ).FirstOrDefault();

                    if (validar != null)
                    {
                        throw new MensageException("El nombre de la cuenta en esta empresa ya existe");
                    }


                    var linqcuenta = (from x in esquema.Cuenta
                                      where x.IdEmpresa == Entidad.IdEmpresa
                                      select x
                                    ).FirstOrDefault();

                    if (linqcuenta == null)
                    {
                        string codigoAux = "";

                        if (empresa != null)
                        {

                            switch (empresa.Niveles)
                            {

                                case 3:

                                    codigoAux = "1.0.0";

                                    break;
                                case 4:
                                    codigoAux = "1.0.0.0";
                                    break;
                                case 5:
                                    codigoAux = "1.0.0.0.0";
                                    break;
                                case 6:
                                    codigoAux = "1.0.0.0.0.0";
                                    break;
                                case 7:
                                    codigoAux = "1.0.0.0.0.0.0";
                                    break;

                                default:
                                    break;
                            }

                            Entidad.Codigo = codigoAux;
                            Entidad.Nivel = 1;

                            esquema.Cuenta.Add(Entidad);
                            esquema.SaveChanges();

                        }



                    }
                    else
                    {




                        if (empresa != null)
                        {

                            if (padre == 0)
                            {
                                //hijos
                                var cuentapadre = (from x in esquema.Cuenta
                                                   where x.IdEmpresa == Entidad.IdEmpresa
                                                   && x.IdCuenta == idcuenta
                                                   select x
                                                  ).FirstOrDefault();

                                if (cuentapadre != null)
                                {

                                    int codigo = 0;
                                    string aux = "";
                                    int encuentra = 0;

                                    var hijos = (from x in esquema.Cuenta
                                                 where x.IdEmpresa == Entidad.IdEmpresa
                                                 && x.IdCuentaPadre == idcuenta
                                                 orderby x.IdCuenta descending
                                                 select x
                                                 ).FirstOrDefault();
                                    if (hijos != null)
                                    {

                                        string[] valores = hijos.Codigo.Split('.');


                                        for (int i = 0; i < valores.Count(); i++)
                                        {

                                            if (i == (hijos.Nivel - 1))
                                            {
                                                // if (valores[i].Equals("0"))
                                                // {
                                                codigo = Int32.Parse(valores[i]) + 1;
                                                encuentra = 1;
                                                valores[i] = codigo.ToString();
                                                //}
                                                //  int l = valores.Count();
                                                if (i == (valores.Count() - 1))
                                                {
                                                    Entidad.TipoDeCuenta = (int)TipoCuenta.Detalle;
                                                }
                                            }

                                            aux = aux + valores[i] + ".";


                                        }

                                        if (encuentra == 0)
                                        {
                                            throw new MensageException("Ultimo nivel de empresa");
                                        }

                                        Entidad.Codigo = aux.Remove(aux.Length - 1);
                                        Entidad.Nivel = hijos.Nivel;

                                        esquema.Cuenta.Add(Entidad);
                                        esquema.SaveChanges();

                                  

                                    }
                                    else
                                    {

                                        string[] valores = cuentapadre.Codigo.Split('.');


                                        for (int i = 0; i < valores.Count(); i++)
                                        {

                                            if (i == (cuentapadre.Nivel))
                                            {
                                                // if (valores[i].Equals("0"))
                                                // {
                                                codigo = Int32.Parse(valores[i]) + 1;
                                                encuentra = 1;
                                                valores[i] = codigo.ToString();
                                                //}

                                                if (i == (valores.Count() - 1))
                                                {
                                                    Entidad.TipoDeCuenta = (int)TipoCuenta.Detalle;
                                                }
                                            }



                                            aux = aux + valores[i] + ".";


                                        }

                                        if (encuentra == 0)
                                        {
                                            throw new MensageException("Ultimo nivel de empresa");
                                        }

                                        Entidad.Codigo = aux.Remove(aux.Length - 1);
                                        Entidad.Nivel = cuentapadre.Nivel + 1;

                                        esquema.Cuenta.Add(Entidad);
                                        esquema.SaveChanges();



                                    }


                                }


                            }
                            else if (padre == 1)
                            {
                                //agregar padres

                                var linqpadres = (from x in esquema.Cuenta
                                                  where x.IdEmpresa == Entidad.IdEmpresa
                                                  && x.Nivel == 1
                                                  orderby x.IdCuenta descending
                                                  select x
                                                 ).FirstOrDefault();

                                if (linqpadres != null)
                                {
                                    int codigo = 0;
                                    string aux = "";
                                    int encuentra = 0;


                                    string[] valores = linqpadres.Codigo.Split('.');


                                    for (int i = 0; i < valores.Count(); i++)
                                    {

                                        if (i == (linqpadres.Nivel - 1))
                                        {
                                            // if (valores[i].Equals("0"))
                                            // {
                                            codigo = Int32.Parse(valores[i]) + 1;
                                            encuentra = 1;
                                            valores[i] = codigo.ToString();
                                            //}
                                        }

                                        aux = aux + valores[i] + ".";


                                    }

                                    if (encuentra == 0)
                                    {
                                        throw new MensageException("Ultimo nivel de empresa");
                                    }

                                    Entidad.Codigo = aux.Remove(aux.Length - 1);
                                    Entidad.Nivel = linqpadres.Nivel;

                                    esquema.Cuenta.Add(Entidad);
                                    esquema.SaveChanges();

                                }


                            }


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
        public List<ERDatosBasicoCuenta> ReporteDatosBasicoCuenta(string usuario, string empresa)
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
        }
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
                throw new MensageException("Existen cuentas repetidas en la configuracion");
            }

        }
    }
}
