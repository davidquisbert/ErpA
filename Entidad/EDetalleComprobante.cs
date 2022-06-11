using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EComprobante
    {
        public int Serie { get; set; }
        public string Estado { get; set; }
        public string TipoComprobante { get; set; }
        public string Moneda { get; set; }
        public string Fecha { get; set; }
        public string Glosa { get; set; }
        public double TipoCambio { get; set; }
    }
    public class EDetalleComprobante
    {
        public int IdDetalle { get; set; }
        public int IdCuenta { get; set; }
        public string Cuenta { get; set; }
        public string Glosa { get; set; }
        public double Debe { get; set; }
        public double Haber { get; set; }

    }
    public class EDetalleTotal
    {
        public double TotalDebe { get; set; }
        public double TotalHaber { get; set; }
    }
    public class DetalleEstado
    {
        public int Estado { get; set; }
    }
    public class ESumasSaldos
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public string Fecha { get; set; }
        public double SumasDebe { get; set; }
        public double SumasHaber { get; set; }
        public double SaldosDebe { get; set; }
        public double SaldoHaber { get; set; }

    }
    public class EDetalleTotalSumasSaldos
    {
        public double TotalSumasDebe { get; set; }
        public double TotalSumasHaber { get; set; }
        public double TotalSaldosDebe { get; set; }
        public double TotalSaldoHaber { get; set; }
    }
    public class ELibroDiario
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public string Fecha { get; set; }
        public double Debe { get; set; }
        public double Haber { get; set; }
    }
    public class ESumasySaldos
    {
        public string Cuenta { get; set; }
        public double SumaDebe { get; set; }
        public double SumaHaber { get; set; }
        public double SaldoDebe { get; set; }
        public double SaldoHaber { get; set; }
    }
    public class ELibroMayoCabezera
    {
        public int IdCuenta { get; set; }
        public string Cuenta { get; set; }

    }
    public class ELibroMayor
    {

        public int IdCuenta { get; set; }
        public string Cuenta { get; set; }
        public string Fecha { get; set; }
        public int NroComprobante { get; set; }
        public string Tipo { get; set; }
        public string Glosa { get; set; }
        public double Debe { get; set; }
        public double Haber { get; set; }
        public double Saldo { get; set; }
    }
    //BALANCE INICIAL
    public class EBalanceInicialActivo
    {
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Debe { get; set; }
        public double TotalGeneral { get; set; }
    }
    public class ECabeceraActivo
    {
        public double Totalactivo { get; set; }
        public string Codigoactivo { get; set; }
        public string Cuentaactivo { get; set; }
    }
    public class EBalanceInicialPasivoPatrimonio
    {
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalGeneral { get; set; }

    }
    public class ECabeceraPasivoPatrimonio
    {
        public double Totalpasivopatrimonio { get; set; }
        public string Codigopasivo { get; set; }
        public string Cuentapasivo { get; set; }
        public string Codigopatrimonio { get; set; }
        public string Cuentapatrimonio { get; set; }
    }
    public class EBalanceInicialCabecera1
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera1{ get; set; }

    }
    public class EBalanceInicialCabecera2
    {

        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera2 { get; set; }
    }
    public class EDetalleTotalBalance
    {
        public double TotalCuentadebe { get; set; }
        public double TotalCuentahaber { get; set; }
        
    }
    //BALANCE GENERAL
    public class EBalanceGeneralActivo
    {
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Debe { get; set; }
        public double TotalGeneral { get; set; }
    }
    public class ECabeceraGeneralActivo
    {
        public double Totalactivo { get; set; }
        public string Codigoactivo { get; set; }
        public string Cuentaactivo { get; set; }
    }
    public class EBalanceGeneralPasivoPatrimonio
    {
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalGeneral { get; set; }

    }
    public class ECabeceraGeneralPasivoPatrimonio
    {
        public double Totalpasivopatrimonio { get; set; }
        public string Codigopasivo { get; set; }
        public string Cuentapasivo { get; set; }
        public string Codigopatrimonio { get; set; }
        public string Cuentapatrimonio { get; set; }
    }
    public class EBalanceGeneralCabecera1
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera1 { get; set; }

    }
    public class EBalanceGeneralCabecera2
    {

        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera2 { get; set; }
    }
    public class EDetalleTotalBalanceGeneral
    {
        public double TotalCuentadebe { get; set; }
        public double TotalCuentahaber { get; set; }

    }
    //ESTADO DE RESULTADOS
    public class EEstadoResultadosIngreso
    {
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Debe { get; set; }
        public double TotalGeneral { get; set; }
    }
    public class EEstadoResultadosCosto
    {
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalGeneral { get; set; }

    }
    public class EEstadoResultadosGasto
    {
        public int IdCabecera1 { get; set; }
        public string CodigoCabecera1 { get; set; }
        public string CuentaCabecera1 { get; set; }
        public double TotalCabecera1 { get; set; }
        public int IdCabecera2 { get; set; }
        public string CodigoCabecera2 { get; set; }
        public string CuentaCabecera2 { get; set; }
        public double TotalCabecera2 { get; set; }
        public int IdCabecera3 { get; set; }
        public string CodigoCabecera3 { get; set; }
        public string CuentaCabecera3 { get; set; }
        public double TotalCabecera3 { get; set; }
        public int IdCabecera4 { get; set; }
        public string CodigoCabecera4 { get; set; }
        public string CuentaCabecera4 { get; set; }
        public double TotalCabecera4 { get; set; }
        public int IdCabecera5 { get; set; }
        public string CodigoCabecera5 { get; set; }
        public string CuentaCabecera5 { get; set; }
        public double TotalCabecera5 { get; set; }
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double Haber { get; set; }
        public double TotalGeneral { get; set; }

    }
    public class ECabeceraEstadoResultadosIngreso
    {
        public double Totalactivo { get; set; }
        public string Codigoingreso { get; set; }
        public string Cuentaingreso { get; set; }
    }
    public class ECabeceraEstadoResultadosCosto
    {
        public double Totalpasivopatrimonio { get; set; }
        public string Codigocosto { get; set; }
        public string Cuentacosto { get; set; }
    }
    public class ECabeceraEstadoResultadosGasto
    {
        public double Totalpasivopatrimonio { get; set; }
        public string Codigogasto { get; set; }
        public string Cuentagasto { get; set; }
    }
    public class EEstadoResultadosCabecera1
    {
        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera1 { get; set; }

    }
    public class EEstadoResultadosCabecera2
    {

        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera2 { get; set; }
    }
    public class EEstadoResultadosCabecera3
    {

        public string CodigoCuenta { get; set; }
        public string Cuenta { get; set; }
        public double TotalCabecera2 { get; set; }
    }
    public class EDetalleTotalEstadoResultados
    {
        public double TotalCuentadebe { get; set; }
        public double TotalCuentahaber { get; set; }

    }
}
