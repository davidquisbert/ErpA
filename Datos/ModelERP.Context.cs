//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ERPBDEntities : DbContext
    {
        public ERPBDEntities()
            : base("name=ERPBDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<ArticuloCategoria> ArticuloCategoria { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Comprobante> Comprobante { get; set; }
        public virtual DbSet<Cuenta> Cuenta { get; set; }
        public virtual DbSet<DetalleComprobante> DetalleComprobante { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EmpresaMoneda> EmpresaMoneda { get; set; }
        public virtual DbSet<Gestion> Gestion { get; set; }
        public virtual DbSet<Moneda> Moneda { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Lote> Lote { get; set; }
        public virtual DbSet<Nota> Nota { get; set; }
        public virtual DbSet<Detalle> Detalle { get; set; }
    }
}
