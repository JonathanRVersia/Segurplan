using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.Detail;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Update {
    public class UpdateRiskAndPreventiveMeasuresRequestHandler : IRequestHandler<UpdateRiskAndPreventiveMeasuresRequest, IRequestResponse<UpdateRiskAndPreventiveMeasuresResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public UpdateRiskAndPreventiveMeasuresRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        private RisksAndPreventiveMeasures DatabaseData;
        private RisksAndPreventiveMeasures RiskAndPreventiveMeasure;


        public async Task<IRequestResponse<UpdateRiskAndPreventiveMeasuresResponse>> Handle(UpdateRiskAndPreventiveMeasuresRequest request, CancellationToken cancellationToken) {

            bool riskIsAlreadyAsigned = context.RisksAndPreventiveMeasures
                                                  .Any(x => x.ActivityId == request.RiskAndPreventiveMeasures.ActivityId
                                                        &&
                                                        x.RiskId == request.RiskAndPreventiveMeasures.RiskId && x.Id != request.RiskAndPreventiveMeasures.Id);

            if (riskIsAlreadyAsigned) {
                return RequestResponse.Error<UpdateRiskAndPreventiveMeasuresResponse>(new Exception("riskIsAlreadyAsigned"));
            }


            bool reorderNeeded = context.RisksAndPreventiveMeasures.Any(
                   x => x.ChapterId == request.RiskAndPreventiveMeasures.ChapterId
                   &&
                   x.SubChapterId == request.RiskAndPreventiveMeasures.SubChapterId
                   &&
                   x.ActivityId == request.RiskAndPreventiveMeasures.ActivityId
                   &&
                   x.RiskId == request.RiskAndPreventiveMeasures.RiskId);

            if (reorderNeeded) {
                await ReorderRiskAndPreventiveMeasures(request);
            }


            if (request.RiskAndPreventiveMeasures.Id != default) {

                await UpdateBehaviorAsync(request);

            } else {
                await CreateBehaviorAsync(request);

            }
            int changes = await context.SaveChangesAsync();

            if (changes < 1)
                return RequestResponse.NotFound<UpdateRiskAndPreventiveMeasuresResponse>();

            return RequestResponse.Ok(new UpdateRiskAndPreventiveMeasuresResponse(DatabaseData.Id));

        }

        private async Task<int> CreateBehaviorAsync(UpdateRiskAndPreventiveMeasuresRequest request) {

            RiskAndPreventiveMeasure = mapper.Map<RisksAndPreventiveMeasures>(request.RiskAndPreventiveMeasures);


            if (request.RiskAndPreventiveMeasures.PreventiveMeasures.Any()) {
                RiskAndPreventiveMeasure.PreventiveMeasures = await getDbPreventiveMeasures(request);
                foreach (var item in RiskAndPreventiveMeasure.PreventiveMeasures) {
                    item.Id = 0;
                    item.PreventiveMeasureOrder = request.RiskAndPreventiveMeasures.PreventiveMeasures.FirstOrDefault(x => x.Id == item.PreventiveMeasureId)?.Order ?? 0;
                }
            }

            RiskAndPreventiveMeasure.CreateDate = DateTime.Now;
            RiskAndPreventiveMeasure.UpdateDate = DateTime.Now;
            RiskAndPreventiveMeasure.CreatedBy = request.UserId;
            RiskAndPreventiveMeasure.ModifiedBy = request.UserId;

            context.RisksAndPreventiveMeasures.Add(RiskAndPreventiveMeasure);

            DatabaseData = RiskAndPreventiveMeasure;
            return RiskAndPreventiveMeasure.Id;
        }

        private async Task UpdateBehaviorAsync(UpdateRiskAndPreventiveMeasuresRequest request) {


            var oldMeasures = context.RiskAndPreventiveMeasuresMeasures.Where(x => x.RisksAndPreventiveMeasuresId == request.RiskAndPreventiveMeasures.Id).ToList();
            context.RemoveRange(oldMeasures);
            context.SaveChanges();

            foreach (var oldMeasure in oldMeasures) {
                context.Entry(oldMeasure).State = EntityState.Detached;
            }

            DatabaseData = context.RisksAndPreventiveMeasures.First(x => x.Id == request.RiskAndPreventiveMeasures.Id);
            RiskAndPreventiveMeasure = mapper.Map(request.RiskAndPreventiveMeasures, DatabaseData);
            context.Entry(DatabaseData).State = EntityState.Detached;


            if (request.RiskAndPreventiveMeasures.PreventiveMeasures.Any()) {
                RiskAndPreventiveMeasure.PreventiveMeasures = await getDbPreventiveMeasures(request);
                foreach(var preventiveMeasures in RiskAndPreventiveMeasure.PreventiveMeasures) {
                    preventiveMeasures.PreventiveMeasureOrder = request.RiskAndPreventiveMeasures.PreventiveMeasures.FirstOrDefault(x => x.Id == preventiveMeasures.PreventiveMeasureId)?.Order ?? 0;                    
                }
            }

            RiskAndPreventiveMeasure.UpdateDate = DateTime.Now;
            RiskAndPreventiveMeasure.ModifiedBy = request.UserId;
            context.RisksAndPreventiveMeasures.Update(RiskAndPreventiveMeasure);
        }

        private async Task<List<RiskAndPreventiveMeasuresMeasures>> getDbPreventiveMeasures(UpdateRiskAndPreventiveMeasuresRequest request) {
            var preventiveMeasuresToAddIds = request.RiskAndPreventiveMeasures.PreventiveMeasures.Select(x => x.Id);
            List<int> selectedMeasuresIds = new List<int>();
            //var selectedMeasuresIds = await context.PreventiveMeasure.Where(x => preventiveMeasuresToAddIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            //Este foreach se hace para mantener el orden que tienen en front
            foreach (var prevMeas in preventiveMeasuresToAddIds) {
                var IdSelected = await context.PreventiveMeasure.Where(x => x.Id == prevMeas).FirstOrDefaultAsync();
                if(IdSelected!=null && IdSelected.Id > 0) {
                    selectedMeasuresIds.Add(IdSelected.Id);
                }
            }

            List<RiskAndPreventiveMeasuresMeasures> riskAndPreventiveMeasuresMeasures = new List<RiskAndPreventiveMeasuresMeasures>();
            foreach (var selectedMeasureId in selectedMeasuresIds) {
                riskAndPreventiveMeasuresMeasures.Add(new RiskAndPreventiveMeasuresMeasures { PreventiveMeasureId = selectedMeasureId, RisksAndPreventiveMeasuresId = request.RiskAndPreventiveMeasures.Id });
            }

            return riskAndPreventiveMeasuresMeasures;
        }

        private async Task ReorderRiskAndPreventiveMeasures(UpdateRiskAndPreventiveMeasuresRequest request) {

            IQueryable<RisksAndPreventiveMeasures> storedRiskAndPreventiveMeasures = context.RisksAndPreventiveMeasures.Where(
                     x => x.ChapterId == request.RiskAndPreventiveMeasures.ChapterId
                     &&
                     x.SubChapterId == request.RiskAndPreventiveMeasures.SubChapterId
                     &&
                     x.ActivityId == request.RiskAndPreventiveMeasures.ActivityId
                     &&
                     x.RiskId == request.RiskAndPreventiveMeasures.RiskId);


            List<RisksAndPreventiveMeasures> queryResult = new List<RisksAndPreventiveMeasures>();


            if (request.RiskAndPreventiveMeasures.Id != default) {

                int oldPosition = context.RisksAndPreventiveMeasures.FirstOrDefault(x => x.Id == request.RiskAndPreventiveMeasures.Id).RiskOrder;

                if (request.RiskAndPreventiveMeasures.RiskOrder > oldPosition) {
                    queryResult = storedRiskAndPreventiveMeasures.Where(prevMeasure => prevMeasure.RiskOrder <= request.RiskAndPreventiveMeasures.RiskOrder
                                                                            && prevMeasure.RiskOrder > oldPosition - 1).OrderByDescending(prevMeasure => prevMeasure.RiskOrder).ToList();
                    ApplyOrder(queryResult, false);

                } else {
                    queryResult = storedRiskAndPreventiveMeasures.Where(prevMeasure => prevMeasure.RiskOrder >= request.RiskAndPreventiveMeasures.RiskOrder
                                                                        && prevMeasure.RiskOrder < oldPosition + 1).OrderBy(prevMeasure => prevMeasure.RiskOrder).ToList();
                    ApplyOrder(queryResult, true);

                }
            } else {
                queryResult = storedRiskAndPreventiveMeasures.Where(prevMeasure => prevMeasure.RiskOrder >= request.RiskAndPreventiveMeasures.RiskOrder).OrderBy(prevMeasure => prevMeasure.RiskOrder).ToList();
                ApplyOrder(queryResult, true);

            }

            context.UpdateRange(queryResult);
            await context.SaveChangesAsync();


            foreach (var oldMeasure in queryResult) {
                context.Entry(oldMeasure).State = EntityState.Detached;
            }

            void ApplyOrder(List<RisksAndPreventiveMeasures> preventiveMeasures, bool isSum) {
                preventiveMeasures.Select(prevMes => {
                    if (isSum) {
                        prevMes.RiskOrder++;
                    } else {
                        prevMes.RiskOrder--;
                    }
                    return prevMes;
                }).ToList();

            }


        }
    }
}
