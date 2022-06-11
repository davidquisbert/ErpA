using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ENota
    {
        public int NroNota { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public int Tipo { get; set; }
        public string TipoNota { get; set; }
        public string Estado { get; set; }
    }
    public class ELote
    {
        public int IdArticulo { get; set; }
        public int NroLote { get; set; }
        public int idlote { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaVencimiento { get; set; }
        public string Articulo { get; set; }
        public int Cantidad { get; set; }
        public double PrecioCompra { get; set; }
        public int Stock { get; set; }
        public double SubTotal { get; set; }
        public int IdNota { get; set; }
        public int Estado { get; set; }

    }
    public class EDetalle
    {
        public int iddetalle { get; set; }
        public int IdArticulo { get; set; }
        public int IdNota { get; set; }
        public int NroLote { get; set; }
        public int Cantidad { get; set; }
        public double PrecioVenta { get; set; }
        public string Articulo { get; set; }
        public double SubTotal { get; set; }
    }
    public class ENotaTotal
    {
        public double Total { get; set; }
    }
    public class ELoteSubtotal
    {
        public double Subtotal { get; set; }
    }
    public class NotaEstado
    {
        public int Estado { get; set; }
    }
    public class LoteEstado
    {
        public int Estado { get; set; }
    }
    public class ELoteJSON
    {
        
        public int NroLote { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaVencimiento { get; set; }
        public int Cantidad { get; set; }
        public double PrecioCompra { get; set; }
        public int Stock { get; set; }
        public int Estado { get; set; }
    }
    public class ENotasJSON
    {
        public int NroNota { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public int Tipo { get; set; }
        public string TipoNota { get; set; }
        public string Estado { get; set; }
    }
    public class EPeriodosJSON
    {
        public string Descripcion { get; set; }
    }
    public class EProductosBajoStockJSON
    {
        public string Descripcion { get; set; }
        public int Stock { get; set; }
    }
}
