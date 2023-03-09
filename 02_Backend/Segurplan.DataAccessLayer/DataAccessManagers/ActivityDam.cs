
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers.DamBase;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class ActivityDam : SegurplanDamBase {
        public const string OrderByChapter = "Chapter";
        public const string OrderBySubchapter = "Subchapter";
        public const string OrderByActivityDescription = "Description";
        public const string OrderByActivityNumber = "Number";
        public const string OrderByActivityVersion = "Version";

        public const string FilterByChapter = OrderByChapter;
        public const string FilterBySubchapter = OrderBySubchapter;
        public const string FilterByActivityDescription = OrderByActivityDescription;
        public const string FilterByActivityNumber = OrderByActivityNumber;
        public const string FilterByActivityVersion = OrderByActivityVersion;


        public ActivityDam(SegurplanContext context) : base(context) {

        }
        public async Task<int> CreateActivityAsync(Activity activity) {

            context.Activity.Add(activity);
            return await context.SaveChangesAsync() > 0 ? activity.Id : -1;
        }
        public async Task<IQueryable<Activity>> SelectAllActivitys() => (from x in context.Activity select x);
        public async Task<IQueryable<Chapter>> SelectAllChapters() => (from x in context.Chapter select x);
        public async Task<IQueryable<SubChapter>> SelectAllSubchapters() => (from x in context.SubChapter select x);
        //public async Task<IQueryable<ChapterVersionInfo>> SelectAllChapterVersions() => (from x in context.ChapterVersionInfo select x);

    }
}
