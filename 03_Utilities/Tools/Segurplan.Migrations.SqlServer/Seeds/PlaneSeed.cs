//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Segurplan.Core.Database;
//using Segurplan.DataAccessLayer.Database.DataTransferObjects;
//using Segurplan.Migrations.SqlServer.Helpers;

//namespace Segurplan.Migrations.SqlServer.Seeds {
//    public class PlaneSeed : IDataSeed {
//        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

//            List<Plane> planes = await GetBlueprinsWithPlanes();

//            await context.AddRangeAsync(planes);
//            context.Database.OpenConnection();
//            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Plane ON");
//            await context.SaveChangesAsync();
//            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Plane OFF");
//            context.Database.CloseConnection();
//        }

//        private async Task<List<Plane>> GetBlueprinsWithPlanes() {

//            List<Plane> planes = PlaneData.Planes;
//            string path = @"..\..\..\Helpers\DefaultBlueprints\";
//            DirectoryInfo d = new DirectoryInfo(path);
//            FileInfo[] Files = d.GetFiles("*.jpg");
//            bool canSetToPlan;
//            string fileCode = "";

//            foreach (FileInfo file in Files) {

//                fileCode = file.Name.Substring(0, file.Name.IndexOf(" "));
//                canSetToPlan = planes.Any(plane => plane.Name.Substring(0, plane.Name.IndexOf(" ")) == fileCode);

//                if (canSetToPlan)
//                    planes.Find(plane => plane.Name.Substring(0, plane.Name.IndexOf(" ")) == fileCode).Data = File.ReadAllBytes(file.ToString());
//            }

//            return planes;
//        }
//    }
//}
