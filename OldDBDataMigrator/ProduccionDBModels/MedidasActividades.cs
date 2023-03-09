using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class MedidasActividades
    {
        public int Id { get; set; }
        public int? Capitulo { get; set; }
        public int? Subcapitulo { get; set; }
        public string Actividad { get; set; }
        public string Titulo { get; set; }
        public int? IdMedida { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? Idaux { get; set; }
        public int? IdauxPadre { get; set; }
        public string Texto { get; set; }
        public string DescripcionWord { get; set; }
        public int? Posicion { get; set; }

        public Medidas IdMedidaNavigation { get; set; }
    }
}
