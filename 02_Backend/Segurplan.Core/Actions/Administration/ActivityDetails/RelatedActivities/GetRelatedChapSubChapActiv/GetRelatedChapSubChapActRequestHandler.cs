using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.GetRelatedChapSubChapActiv {
    public class GetRelatedChapSubChapActRequestHandler : IRequestHandler<GetRelatedChapSubChapActRequest, IRequestResponse<GetRelatedChapSubChapActResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public GetRelatedChapSubChapActRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<GetRelatedChapSubChapActResponse>> Handle(GetRelatedChapSubChapActRequest request, CancellationToken cancellationToken) {
            var relatedActivities = await context.ActivityRelations.ProjectTo<ActivityRelationsModel>(mapper.ConfigurationProvider).Where(x => x.IdRelations == request.IdRelations).ToListAsync();
            var relatedActivitiesCopy = relatedActivities.ToList();
            if (RequestResponse.Ok().Status == RequestStatus.Ok) {
                foreach (var item in relatedActivities) {
                    if (item.IdActivityRelation != 0) {
                        item.IdSubChapterRelation = GetSubChap(item.IdActivityRelation).Result;
                        item.IdChapterRelation = GetChap(item.IdSubChapterRelation).Result;
                    } else if (item.IdSubChapterRelation!= 0) {
                        var chap = GetChap(item.IdSubChapterRelation).Result;
                        var subchap = GetSubChapterVersion(item.IdSubChapterRelation).Result;
                        var listAct = GetActivities(subchap).Result;
                        foreach (var x in listAct) {
                            relatedActivitiesCopy.Add(new ActivityRelationsModel {IdRelations= request.IdRelations, IdActivityRelation = x,IdChapterRelation = chap, IdSubChapterRelation = item.IdSubChapterRelation });
                        }
                        var itemToRemove = relatedActivitiesCopy.Where(x => x.IdSubChapterRelation == item.IdSubChapterRelation && x.IdActivityRelation == 0).FirstOrDefault();
                        relatedActivitiesCopy.Remove(itemToRemove);
                    } else if (item.IdChapterRelation!=0) {
                        var chap = GetChapVersion(item.IdChapterRelation).Result;
                        var listSubChap = GetSubChapters(chap).Result;
                        foreach (var x in listSubChap) {
                            var subChap = GetSubChapterVersion(x).Result;
                            var listAct = GetActivities(subChap).Result;
                            foreach (var y in listAct) {
                                relatedActivitiesCopy.Add(new ActivityRelationsModel { IdRelations = request.IdRelations,  IdActivityRelation = y, IdChapterRelation = item.IdChapterRelation, IdSubChapterRelation = x });
                            }
                        }
                        var itemToRemove = relatedActivitiesCopy.Where(x => x.IdChapterRelation == item.IdChapterRelation && x.IdActivityRelation == 0 && x.IdSubChapterRelation == 0).FirstOrDefault();
                        relatedActivitiesCopy.Remove(itemToRemove);
                    }
                }
            }
            relatedActivities = relatedActivitiesCopy.Where(c => c != null).ToList();
            return RequestResponse.Ok(new GetRelatedChapSubChapActResponse(relatedActivities));
        }

        public async Task<int> GetChapVersion(int chap) {
            int chapterVersion = await context.ChapterVersion.Where(x => x.IdChapter == chap && x.ApprovementDate < DateTime.Now && x.EndDate == null || x.EndDate > DateTime.Now).Select(c=>c.Id).FirstOrDefaultAsync();
            return chapterVersion;
        }
        public async Task<int> GetChap(int subChap) {
            int chapVersion= await context.SubChapterVersion.Where(x => x.IdSubChapter == subChap).Select(z=>z.IdChapterVersion).FirstOrDefaultAsync();
            int chap = await context.ChapterVersion.Where(x => x.Id == chapVersion).Select(z => z.IdChapter).FirstOrDefaultAsync();
            return chap;
        }
        public async Task<int> GetSubChap(int activity) {
            int subChapVersion = await context.ActivityVersion.Where(x => x.IdActivity == activity).Select(z=>z.IdSubChapterVersion).FirstOrDefaultAsync();
            int subChap = await context.SubChapterVersion.Where(x => x.Id == subChapVersion).Select(z => z.IdSubChapter).FirstOrDefaultAsync();
            return subChap;
        }
        public async Task<List<int>> GetActivities(int subChap) {
            List<int> list = await context.ActivityVersion.Where(x => x.IdSubChapterVersion == subChap).Select(z=>z.IdActivity).ToListAsync();
            return list;
        }

        public async Task<List<int>> GetSubChapters(int chap) {
            List<int> list = await context.SubChapterVersion.Where(x => x.IdChapterVersion == chap).Select(z => z.IdSubChapter).ToListAsync();
            return list;
        }
        public async Task<int> GetSubChapterVersion(int subChap) {
            int subChapterVersion = await context.SubChapterVersion.Where(x => x.IdSubChapter == subChap).Select(z=>z.Id).FirstOrDefaultAsync();
            return subChapterVersion;
        }
    }
}
