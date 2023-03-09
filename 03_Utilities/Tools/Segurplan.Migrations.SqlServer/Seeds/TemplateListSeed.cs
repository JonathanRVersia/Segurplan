//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Segurplan.Core.Database;
//using Segurplan.DataAccessLayer.Database.DataTransferObjects;

//namespace Segurplan.Migrations.SqlServer.Seeds {
//    public class TemplateListSeed : IDataSeed {
//        List<Template> templates = new List<Template> {
//    //new Template {Name="Plan de seguridad",Notes="plantilla para el plan",FilePath="PLAN DE SEGURIDAD.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-07-30T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-11-17T00:00:00.000")},
//    new Template {Name="Plan formato sin tablas completo",Notes="Parte de la plantilla 1 para tipo Evaluación",FilePath="Plan formato sin tablas completo.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-03T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-13T00:00:00.000")},
//    new Template {Name="Plan nuevo formato tabla",Notes="plantilla para la generación del documento excel del presupuesto",FilePath="Plan nuevo formato tabla.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-03T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-03T00:00:00.000")},
//    new Template {Name="Plan SS corto EBS",Notes="Portada",FilePath="Plan SS corto EBS.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},
//    new Template {Name="Plan SS corto ESS",Notes="Tabla de actividades",FilePath="Plan SS corto ESS.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},
//    new Template {Name="Plan SS largo EBS 1",Notes="Tabla de actividades",FilePath="Plan SS largo EBS 1.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},
//    //new Template {Name="Plan SS largo EBS 2",Notes="Tabla de actividades",FilePath="Plan SS largo EBS 2.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},
//    new Template {Name="Plan SS largo ESS 1",Notes="Tabla de actividades",FilePath="Plan SS largo ESS 1.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},
//    //new Template {Name="Plan SS largo ESS 2",Notes="Tabla de actividades",FilePath="Plan SS largo ESS 2.docx",CreatedBy=218,CreateDate=Convert.ToDateTime("2009-08-11T00:00:00.000"),ModifiedBy=218,UpdateDate=Convert.ToDateTime("2009-08-11T00:00:00.000")},

//    };

//        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {
//            await context.Template.AddRangeAsync(templates);
//            context.SaveChanges();
//        }
//    }
//}

