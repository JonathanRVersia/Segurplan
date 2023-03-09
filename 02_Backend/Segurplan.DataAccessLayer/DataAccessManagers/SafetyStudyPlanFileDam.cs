using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class SafetyStudyPlanFileDam : SegurplanDamBase {
        public SafetyStudyPlanFileDam(SegurplanContext context) : base(context) {
        }

        //Returns a int array if plan gots atached files, empty array if no files 
        //and null on error
        public async Task<int[]> PlanFileIDList(int planId) {


            return await Task.Run(() => {
                try {
                    var query = from z in context.SafetyStudyPlanFile
                                where z.IdSafetyStudyPlan == planId
                                select z.Id;
                    var response = query.Count() > 0 ? query.ToArray() : new int[0];

                    return response;

                } catch (Exception epa) {

                    Debug.WriteLine(epa.ToString());
                    return null;
                }
            });
        }

        //returns file inserted id on success, -1 on fail
        public async Task<int> SaveFile(int userId, SafetyStudyPlanFile file) {
            try {

                file.CreatedBy = userId;
                file.ModifiedBy = userId;
                file.CreateDate = DateTime.Now;
                file.UpdateDate = DateTime.Now;

                await context.SafetyStudyPlanFile.AddAsync(file);

                var insertedRows = await context.SaveChangesAsync().ConfigureAwait(true);

                if (insertedRows > 0) {
                    return file.Id;
                } else {
                    return -1;
                }
            } catch (Exception e) {

                Debug.WriteLine(e.ToString());
                return -1;
            }


        }

        public async Task<List<SafetyStudyPlanFile>> SelectByPlanId(int idPlan, int planFileType = 1, bool getData = false) {
            try {
                return (from f in context.SafetyStudyPlanFile
                        where f.IdSafetyStudyPlan == idPlan && f.IdPlanFileType == planFileType
                        select f).ToList();
            } catch (Exception e) {

                Debug.WriteLine(e.ToString());
                return new List<SafetyStudyPlanFile>(0);
            }
        }

        public async Task<Dictionary<string, byte[]>> GetFile(int fileID) {
            try {
                return (from f in context.SafetyStudyPlanFile
                        where f.Id == fileID
                        select new { f.FileName, f.FileData }).ToDictionary(f => f.FileName, f => f.FileData);

            } catch (Exception e) {

                Debug.WriteLine(e.ToString());
                return null;
            }

        }

        public async Task<bool> DeleteFileByIDAsync(int fileID) {

            var planFileObject = (from y in context.SafetyStudyPlanFile
                                  where y.Id == fileID
                                  select y).FirstOrDefault();

            if (planFileObject != null) {

                context.SafetyStudyPlanFile.Remove(planFileObject);
                var numRegistros = await context.SaveChangesAsync().ConfigureAwait(true);
                if (numRegistros == 1) {

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteFilesByPlanId(int planId) {
            var planFiles = from f in context.SafetyStudyPlanFile
                            where f.IdSafetyStudyPlan == planId
                            select f;

            context.RemoveRange(planFiles);

            try {
                context.SaveChangesAsync();
            } catch (Exception e) {
                Console.WriteLine($"Error deleting plan files :: {e.ToString()}");

                throw e;
            };

            return true;
        }
    }
}
