//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Segurplan.Core.Database;
//using Segurplan.DataAccessLayer.Database.DataTransferObjects;
//using Segurplan.Migrations.SqlServer.Helpers;

//namespace Segurplan.Migrations.SqlServer.Seeds {
//    public class PlaneFamilySeed : IDataSeed {
//        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

//            await context.AddRangeAsync(PlaneFamilyData.PlaneFamilies);
//            context.Database.OpenConnection();
//            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.PlaneFamily ON");
//            await context.SaveChangesAsync();
//            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.PlaneFamily OFF");
//            context.Database.CloseConnection();
//        }
//    }
//}
