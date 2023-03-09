using System.Linq;
using System;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class PreventiveMeasureDam : SegurplanDamBase {
        public const string OrderByCode = "Code";
        public const string OrderByMeasure = "Measure";
        public const string FilterByCode = "Code";
        public const string FilterByMeasure = "Measure";
        public PreventiveMeasureDam(SegurplanContext context) : base(context) {

        }
        public async Task<int> CreateMeasureAsync(PreventiveMeasure measure) {
            GenerateCodeForMeasure(measure);
            context.PreventiveMeasure.Add(measure);
            return await context.SaveChangesAsync() > 0 ? measure.Id : -1;
        }

        private void GenerateCodeForMeasure(PreventiveMeasure measure) {
            int biggestCode = context.PreventiveMeasure.Max(x => x.Code);
            measure.Code = biggestCode + 1;
        }

        public async Task<IQueryable<PreventiveMeasure>> SelectAll() => (from m in context.PreventiveMeasure
                                                                         select new PreventiveMeasure {
                                                                             Id = m.Id,
                                                                             Code = m.Code,
                                                                             Description = m.Description
                                                                         }
        );
        public async Task<IQueryable<PreventiveMeasure>> ApplyFilters(IQueryable<PreventiveMeasure> query, string filterName, string filterValue) {
            return filterName.Equals(FilterByCode) ? (from x in query where (Convert.ToInt32(x.Code)) == (Convert.ToInt32(filterValue)) select x) : (from x in query where x.Description.Contains(filterValue) select x);
        }
        public async Task<IQueryable<PreventiveMeasure>> GetListOffset(IQueryable<PreventiveMeasure> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }
        public async Task<IQueryable<PreventiveMeasure>> OrderList(IQueryable<PreventiveMeasure> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case OrderByCode:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Code)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Code)));


                case OrderByMeasure:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Description)
                         : (from x in query select x).OrderBy(x => x.Description);


                default:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Code)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Code)));

            }
        }
        public async Task<PreventiveMeasure> UpdateMeasure(PreventiveMeasure dbMeasure) {
            context.PreventiveMeasure.Update(dbMeasure);
            var saveOk = await context.SaveChangesAsync().ConfigureAwait(true) > 0 ? true : false;
            if (saveOk) {
                return dbMeasure;
            } else {
                return null;
            }
        }
        public async Task<PreventiveMeasure> SelectByMeasureId(int measureId) {
            return (from x in context.PreventiveMeasure where x.Id == measureId select x).FirstOrDefault();
        }
        public async Task<bool> DeleteMeasureAsync(int measureId) {

            var measure = (from x in context.PreventiveMeasure where x.Id == measureId select x).FirstOrDefault();

            try {
                context.PreventiveMeasure.Remove(measure);
                var result = await context.SaveChangesAsync() > 0 ? true : false;
                return result;

            } catch (Exception) {

                return false;
            }
        }
    }
}
