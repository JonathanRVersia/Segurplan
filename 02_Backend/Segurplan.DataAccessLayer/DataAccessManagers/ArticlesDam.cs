using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class ArticlesDam: SegurplanDamBase {
        public const string OrderById = "Id";
        public const string OrderByValue = "Value";
        public const string FilterByName = "Name";
        public const string FilterByFamily = "Family";

        private const string Id = "Id";
        private const string Name = "Name";
        private const string Family = "Family";
        private const string Percentage = "Percentage";
        private const string TimeOfWork = "TimeOfWork";
        private const string AmortizationTime = "AmortizationTime";
        private const string MinimumUnit = "MinimumUnit";
        private const string Price = "Price";

        public ArticlesDam(SegurplanContext context) : base(context) { }

        public async Task<IQueryable<Article>> SelectAll() {

            return  (from article in context.Article.Include(x=> x.IdArticleFamilyNavigation)
                    join af in context.ArticleFamily on article.IdArticleFamily equals af.Id into applicationUsers
                    from family in applicationUsers.DefaultIfEmpty()
                    select article
            );
        }

        public async Task<IQueryable<Article>> ApplyFilters(IQueryable<Article> query, string filterName, string filterValue) {

            switch (filterName) {
                case FilterByName:
                    return (from x in query where x.Name.Contains(filterValue) select x);
                case FilterByFamily:
                    var familyIds = context.ArticleFamily.Where(family => family.Family.Contains(filterValue)).Select(item => item.Id).ToList();
                    return (from x in query where familyIds.Contains(x.IdArticleFamily) select x);
                default:
                    return query;
            }
        }

        public async Task<IQueryable<Article>> GetListOffset(IQueryable<Article> query, int IndexPage, int PageRows, int Takep) {
            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
        }

        public async Task<IQueryable<Article>> OrderList(IQueryable<Article> query, string orderBy, bool orderModeDesc) {

            switch (orderBy) {
                case Id:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));
                case Name:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.Name)
                         : (from x in query select x).OrderBy(x => x.Name);
                case Family:
                    return orderModeDesc
                         ? (from x in query select x).OrderByDescending(x => x.IdArticleFamilyNavigation.Family)
                         : (from x in query select x).OrderBy(x => x.IdArticleFamilyNavigation.Family);                    
                case Percentage:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Percentage)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Percentage)));
                    
                case TimeOfWork:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToDecimal(x.TimeOfWork)))
                       : (from x in query select x).OrderBy(x => (Convert.ToDecimal(x.TimeOfWork)));
                    
                case AmortizationTime:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToDecimal(x.AmortizationTime)))
                       : (from x in query select x).OrderBy(x => (Convert.ToDecimal(x.AmortizationTime)));
                    
                case MinimumUnit:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.MinimumUnit)))
                       : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.MinimumUnit)));
                    
                case Price:
                    return orderModeDesc
                       ? (from x in query select x).OrderByDescending(x => (Convert.ToDecimal(x.Price)))
                       : (from x in query select x).OrderBy(x => (Convert.ToDecimal(x.Price)));
                    
                default:
                    return orderModeDesc
                        ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Id)))
                        : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Id)));
            }
        }
    }
}
