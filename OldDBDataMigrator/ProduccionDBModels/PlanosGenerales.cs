using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class PlanosGenerales
    {
        public PlanosGenerales()
        {
            PlanesPlanos = new HashSet<PlanesPlanos>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Archivo { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdFamilia { get; set; }

        public Familias IdFamiliaNavigation { get; set; }
        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<PlanesPlanos> PlanesPlanos { get; set; }
    }
}
