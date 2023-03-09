using System.Linq;
using System;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class SeriousnessDam : SegurplanDamBase {
        public const string OrderById = "Id";
        public const string OrderByValue = "Value";
        public const string FilterById = "Id";
        public const string FilterByValue = "Value";

        public SeriousnessDam(SegurplanContext context) : base(context) { }

        public async Task<IQueryable<Seriousness>> SelectAll() => (from m in context.Seriousness
                                                                   select new Seriousness {
                                                                       Id = m.Id,
                                                                       Value = m.Value
                                                                   }
        );

        public async Task<IQueryable<Seriousness>> ApplyFilters(IQueryable<Seriousness> query, string filterName, string filterValue) {
            return filterName.Equals(FilterById) ? (from x in query where (Convert.ToInt32(x.Id)) == (Convert.ToInt32(filterValue)) select x) : (from x in query where x.Value.Contains(filterValue) select x);
        }

        public async Task<IQueryable<Seriousness>> GetListOffset(IQueryable<Seriousness> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public async Task<IQueryable<Seriousness>> OrderList(IQueryable<Seriousness> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case OrderById:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));


                case OrderByValue:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Value)
                         : (from x in query select x).OrderBy(x => x.Value);


                default:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));

            }
        }
    }
}
