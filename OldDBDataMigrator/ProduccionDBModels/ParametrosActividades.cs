using System;
using System.Collections.Generic;

namespace OldDBDataMigrator.ProduccionDBModels
{
    public partial class ParametrosActividades
    {
        public int Id { get; set; }
        public int? Capitulo { get; set; }
        public int? Subcapitulo { get; set; }
        public string Actividad { get; set; }
        public string Titulo { get; set; }
        public int? IdParametro { get; set; }
        public int? Idaux { get; set; }
        public int? IdauxPadre { get; set; }
        public string Texto { get; set; }
        public string DescripcionWord { get; set; }
        public int? Posicion { get; set; }
        public int? IdCapitulo { get; set; }
        public int? IdSubcapitulo { get; set; }
        public int? IdActividad { get; set; }
        public int? IdEstado { get; set; }

        public Parametros IdParametroNavigation { get; set; }
    }
}
