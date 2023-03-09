using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Core.Database;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Microsoft.EntityFrameworkCore;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.ActivityRelationsData {
    public class ActivityRelationsDataRequestHandler : IRequestHandler<ActivityRelationsDataRequest, IRequestResponse<ActivityRelationsDataResponse>> {

        private readonly SegurplanContext context;

        public ActivityRelationsDataRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<ActivityRelationsDataResponse>> Handle(ActivityRelationsDataRequest request, CancellationToken cancellationToken) {
            List<ActivityRelationsModel> definitiveDataList = new List<ActivityRelationsModel>();
            foreach(var element in request.RelationsDataList) {
                if(!definitiveDataList.Any(z=>z.IdChapterRelation==element.IdChapterRelation) && !definitiveDataList.Any(z => z.IdSubChapterRelation == element.IdSubChapterRelation)) {
                    int activityCount = 0;
                    List<int> listSubChapter = GetSubChapters(element.IdChapterRelation).Result;
                    foreach(var subchap in listSubChapter) {
                        activityCount+=GetActivityCount(subchap).Result;
                    }
                    int chapCount= EqualValues(element.IdChapterRelation, request.RelationsDataList,"chapter");
                    if (chapCount == activityCount) {
                        definitiveDataList.Add(new ActivityRelationsModel { IdChapterRelation= element.IdChapterRelation, IdSubChapterRelation =0, IdActivityRelation=0});
                    } else {
                        int activityCountSubChapter = 0;
                        activityCountSubChapter = GetActivityCount(element.IdSubChapterRelation).Result;
                        if(activityCountSubChapter== EqualValues(element.IdSubChapterRelation, request.RelationsDataList, "subchapter")) {
                            definitiveDataList.Add(new ActivityRelationsModel { IdSubChapterRelation = element.IdSubChapterRelation, IdChapterRelation = 0, IdActivityRelation = 0 });
                        } else {
                            definitiveDataList.Add(new ActivityRelationsModel { IdActivityRelation = element.IdActivityRelation, IdChapterRelation = 0, IdSubChapterRelation = 0 });
                        }
                    }
                }

            }
            return RequestResponse.Ok(new ActivityRelationsDataResponse(definitiveDataList));
        }

        public async Task<List<int>> GetSubChapters(int chap) {
            int chapterVersion = await context.ChapterVersion.Where(x => x.IdChapter == chap && x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now).Select(c => c.Id).FirstOrDefaultAsync();
            List<int> listSubChapter = await context.SubChapterVersion.Where(x => x.IdChapterVersion == chapterVersion).Select(c => c.IdSubChapter).ToListAsync();
            return listSubChapter;
        }
        public async Task<int> GetActivityCount(int subChap) {
            int subChapterVersion = await context.SubChapterVersion.Where(x => x.IdSubChapter == subChap).Select(c => c.Id).FirstOrDefaultAsync();
            int activityCount = await context.ActivityVersion.Where(x => x.IdSubChapterVersion == subChapterVersion).CountAsync();
            return activityCount;
        }
        public int EqualValues(int Id, List<ActivityRelationsModel> activityRelations,string type) {
            int count = 0;
            if (type=="chapter") {
                count = activityRelations.Where(z => z.IdChapterRelation == Id).Count();
            }
            else if (type=="subchapter") {
                count = activityRelations.Where(z => z.IdSubChapterRelation == Id).Count();
            } 
            else if (type == "activity") {
                count = activityRelations.Where(z => z.IdActivityRelation == Id).Count();
            }
            return count;
        }

        }
}
