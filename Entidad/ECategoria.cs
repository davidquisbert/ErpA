using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ECategoria
    {
        public int idCategoria { get; set; }
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string text { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public Nullable<int> IdCategoriaPadre { get; set; }
        public List<ECategoria> children { get; set; }

    }
    public class ECategoriaJSON
    {
        public int IdCategoria { get; set; }
    }
}

