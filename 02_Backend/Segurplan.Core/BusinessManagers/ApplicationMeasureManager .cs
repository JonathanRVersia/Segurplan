using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.BusinessManagers {

    public class ApplicationMeasureManager {


        private readonly UserDam usrDam;
        private readonly PreventiveMeasureDam measureDam;

        public ApplicationMeasureManager(PreventiveMeasureDam measureDam, UserDam usrDam) {

            this.usrDam = usrDam;
            this.measureDam = measureDam;

        }

        public async Task<int> CreateMeasure(ApplicationPreventiveMeasure measure, int userId) {
            try {
                var dbMeasure = ToPreventiveMeasure(measure);
                if (dbMeasure == null)
                    return -1;

                dbMeasure.CreatedBy = userId;
                dbMeasure.CreateDate = DateTime.Now;
                dbMeasure.ModifiedBy = userId;

                return await measureDam.CreateMeasureAsync(dbMeasure);

            } catch (Exception e) {

                Debug.WriteLine(e.ToString());

                return -1;
            }
        }

        public async Task<ApplicationPreventiveMeasure> UpdateMeasure(ApplicationPreventiveMeasure measure, int userId) {
            //Controlamos el null
            if(measure.Desciption == null) {
                measure.Desciption = "";
            }
            //Aseguramos que tenga <p>
            if (!measure.Desciption.Contains("<p")) {
                measure.Desciption = "<p style=\"font-size:9pt; font-family:Verdana;\">" + measure.Desciption + "</p>";

            }
            var dbMeasure = await measureDam.SelectByMeasureId(measure.Id);
            var usr = usrDam.SelectUserById(userId);

            dbMeasure.UpdateDate = DateTime.Now;
            dbMeasure.Code = measure.Code;
            dbMeasure.Description = measure.Desciption;
            dbMeasure.ModifiedBy = usr.Id;

            return ToApplicationMeasure(await measureDam.UpdateMeasure(dbMeasure));
        }

        public async Task<ApplicationPreventiveMeasure> MeasureData(int measureId) {
            try {

                var response = ToApplicationMeasure(await measureDam.SelectByMeasureId(measureId));

                if (response != null)
                    response.CompleteName = usrDam.CompleteNameFromId(response.CreatedBy);

                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }

        public async Task<bool> DeleteMeasure(int measureId) {
            try {
                return await measureDam.DeleteMeasureAsync(measureId);
            } catch (Exception e) {

                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        private ApplicationPreventiveMeasure ToApplicationMeasure(PreventiveMeasure measure) {

            try {
                return new ApplicationPreventiveMeasure() {
                    Id = measure.Id,
                    Code = measure.Code,
                    Desciption = measure.Description,
                    CreationDate = string.IsNullOrEmpty(measure.CreateDate.ToString()) ? string.Empty : measure.CreateDate.ToString(),
                    ModifiedDate = string.IsNullOrEmpty(measure.UpdateDate.ToString()) ? string.Empty : measure.UpdateDate.ToString(),
                    CreatedBy = measure.CreatedBy
                };

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }



        }

        private PreventiveMeasure ToPreventiveMeasure(ApplicationPreventiveMeasure measure) {

            try {
                return new PreventiveMeasure() {

                    Code = measure.Code,
                    Description = measure.Desciption,

                };

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }



        }

    }



}





