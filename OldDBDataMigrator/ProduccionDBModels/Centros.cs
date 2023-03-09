using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Centros
    {
        public Centros()
        {
            Planes = new HashSet<Planes>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Centro { get; set; }
        public int? IdDelegacion { get; set; }
        public int? IdDireccion { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public Delegaciones IdDelegacionNavigation { get; set; }
        public DireccionesNegocio IdDireccionNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<Planes> Planes { get; set; }
    }
}
