using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class FamilyDam :SegurplanDamBase{

        public const string OrderById = "Id";
        public const string OrderByFamily = "Family";
        public const string FilterById = "Id";
        public const string FilterByFamily = "Family";

        public FamilyDam(SegurplanContext context) : base(context) { }

        public async Task<IQueryable<ArticleFamily>> SelectAll() => (from m in context.ArticleFamily
                                                                     select new ArticleFamily {
                                                                         Id = m.Id,
                                                                         Family = m.Family,
                                                                         CreatedBy = m.CreatedBy,
                                                                         ModifiedBy = m.ModifiedBy,
                                                                         CreateDate = m.CreateDate,
                                                                         UpdateDate = m.UpdateDate
                                                                     });

        public async Task<IQueryable<ArticleFamily>> ApplyFilters(IQueryable<ArticleFamily> query, string filterName, string filterValue) {

            return filterName.Equals(FilterById) ? (from x in query where (Convert.ToInt32(x.Id)) == (Convert.ToInt32(filterValue)) select x) : (from x in query where x.Family.Contains(filterValue) select x);
        }

        public async Task<IQueryable<ArticleFamily>> GetListOffset(IQueryable<ArticleFamily> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public async Task<IQueryable<ArticleFamily>> OrderList(IQueryable<ArticleFamily> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case OrderById:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));

                case OrderByFamily:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Family)
                         : (from x in query select x).OrderBy(x => x.Family);


                default:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));

            }
        }
    }
}
