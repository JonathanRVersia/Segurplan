using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Familias
    {
        public Familias()
        {
            PlanosGenerales = new HashSet<PlanosGenerales>();
        }

        public int Id { get; set; }
        public string Familia { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }

        public ICollection<PlanosGenerales> PlanosGenerales { get; set; }
    }
}
