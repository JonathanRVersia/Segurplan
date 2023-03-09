using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures {
    public class MeasureListRequestHandler : IRequestHandler<MeasureListRequest, IRequestResponse<MeasureListResponse>> {


        protected readonly PreventiveMeasureDam measureDam;

        public MeasureListRequestHandler(PreventiveMeasureDam measureDam) {
            this.measureDam = measureDam;
        }


        public async Task<IRequestResponse<MeasureListResponse>> Handle(MeasureListRequest request, CancellationToken cancellationToken) {


            return await GetPreventiveMeasureList(request);

        }



        private async Task<IRequestResponse<MeasureListResponse>> GetPreventiveMeasureList(MeasureListRequest request) {

            try {
                var manager = new MeasureListManager(measureDam);
                var measureList = await manager.GetPreventiveMeasuresList(request.TableState, request.TableFilter);

                return RequestResponse.Ok(new MeasureListResponse(measureList, manager.FilteredMeasures));

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new MeasureListResponse(null, -1));
            }

        }



    }
}
