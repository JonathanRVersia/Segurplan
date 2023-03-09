using System;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class TasksDam : SegurplanDamBase {
        public const string FilterByName = "Name";

        private const string Id = "Id";
        private const string Name = "Name";

        public TasksDam(SegurplanContext context) : base(context) { }
        public async Task<IQueryable<Tasks>> SelectAll() {

            return (from task in context.Tasks
                    select task);
        }
        public async Task<IQueryable<Tasks>> ApplyFilters(IQueryable<Tasks> query, string filterName, string filterValue) {

            switch (filterName) {
                case FilterByName:
                    return (from x in query where x.Name.Contains(filterValue) select x);
                default:
                    return query;
            }
        }
        public async Task<IQueryable<Tasks>> GetListOffset(IQueryable<Tasks> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public async Task<IQueryable<Tasks>> OrderList(IQueryable<Tasks> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case Id:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));
                case Name:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Name)
                         : (from x in query select x).OrderBy(x => x.Name);

                default:
                    return orderModeDesc
                        ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                        : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));
            }
        }
    }
}
