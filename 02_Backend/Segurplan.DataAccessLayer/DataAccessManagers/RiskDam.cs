using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class RiskDam :SegurplanDamBase{
        public const string OrderById = "Id";
        public const string OrderByName = "Name";
        public const string OrderByCode = "Code";
        public const string FilterByCode = "Code";
        public const string FilterById = "Id";
        public const string FilterByName = "Name";

        public RiskDam(SegurplanContext context) : base(context) { }

        public async Task<IQueryable<Risk>> SelectAll() => (from m in context.Risk
                                                                   select new Risk {
                                                                       Id = m.Id,
                                                                       Code = m.Code,
                                                                       Name = m.Name
                                                                   }
        );

        public async Task<IQueryable<Risk>> ApplyFilters(IQueryable<Risk> query, string filterName, string filterValue) {
            return filterName.Equals(FilterById) ? (from x in query where (Convert.ToInt32(x.Id)) == (Convert.ToInt32(filterValue)) select x) : filterName.Equals(FilterByCode) ? (from x in query where (Convert.ToInt32(x.Code)) == (Convert.ToInt32(filterValue)) select x) : (from x in query where x.Name.Contains(filterValue) select x);
        }

        public async Task<IQueryable<Risk>> GetListOffset(IQueryable<Risk> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public async Task<IQueryable<Risk>> OrderList(IQueryable<Risk> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case OrderById:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));

                case OrderByCode:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Code)))
                         : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Code)));

                case OrderByName:
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
