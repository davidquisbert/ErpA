using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ECuenta
    {

        public int idCuenta { get; set; }
        public int id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string text { get; set; }
        public int TipoCuenta { get; set; }
        public int Nivel { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public Nullable<int> IdCuentaPadre { get; set; }
        public List<ECuenta> children { get; set; }

    }
}
