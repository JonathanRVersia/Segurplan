using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class Medidas
    {
        public Medidas()
        {
            EvaluacionesMedida = new HashSet<EvaluacionesMedida>();
            MedidasActividades = new HashSet<MedidasActividades>();
        }

        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Medida { get; set; }
        public DateTime? FecCreacion { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdEstado { get; set; }
        public int? IdNuevo { get; set; }
        public bool? ExcluirInforme { get; set; }
        public string MedidaSoloTexto { get; set; }

        public Usuarios IdUsuarioNavigation { get; set; }
        public ICollection<EvaluacionesMedida> EvaluacionesMedida { get; set; }
        public ICollection<MedidasActividades> MedidasActividades { get; set; }
    }
}
