using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.BusinessManagers;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures {
    public class MeasureManagementRequestHandler : IRequestHandler<MeasureManagementRequest, IRequestResponse<MeasureManagementResponse>> {


        private ApplicationMeasureManager manager;
        public MeasureManagementRequestHandler(PreventiveMeasureDam measureDam, UserDam userDam) {

            manager = new ApplicationMeasureManager(measureDam, userDam);
        }


        public async Task<IRequestResponse<MeasureManagementResponse>> Handle(MeasureManagementRequest request, CancellationToken cancellationToken) {

            switch (request.CurrentOperation) {
                case AdministrationActionType.Create:
                    return await CreateMeasure(request);
                case AdministrationActionType.Read:
                    return await MeasureInformation(request);
                case AdministrationActionType.Update:
                    return await UpdateMeasureInformation(request);
                case AdministrationActionType.Delete:
                    return await DeleteMeasure(request);

                default:
                    return RequestResponse.NotOk(new MeasureManagementResponse(null, false));
            }

        }

        private async Task<IRequestResponse<MeasureManagementResponse>> CreateMeasure(MeasureManagementRequest request) {
            try {
                var operationOk = await manager.CreateMeasure(request.Measure, request.CurrentUserId) > 0 ? true : false;
                return RequestResponse.Ok(new MeasureManagementResponse(request.Measure, operationOk));

            } catch (Exception e) {

                Debug.WriteLine(e.ToString());

                return RequestResponse.NotOk(new MeasureManagementResponse(null, false));
            }
        }

        private async Task<IRequestResponse<MeasureManagementResponse>> MeasureInformation(MeasureManagementRequest request) {
            try {
                var response = new ApplicationPreventiveMeasure() {
                    Id = 0,
                    Code = 0,
                    Desciption = string.Empty,
                    CreationDate = string.Empty,
                    ModifiedDate = string.Empty,
                    CreatedBy = 0
                };

                if (request.Measure.Id == 0) {// new!!
                    return RequestResponse.Ok(new MeasureManagementResponse(response, true));
                }

                response = await manager.MeasureData(request.Measure.Id);
                return RequestResponse.Ok(new MeasureManagementResponse(response, response == null ? false : true));



            } catch (Exception e) {

                Debug.WriteLine(e.ToString());

                return RequestResponse.NotOk(new MeasureManagementResponse(null, false));
            }
        }

        private async Task<IRequestResponse<MeasureManagementResponse>> UpdateMeasureInformation(MeasureManagementRequest request) {
            try {
                var response = await manager.UpdateMeasure(request.Measure, request.CurrentUserId);
                var operationOk = response != null ? true : false;

                return RequestResponse.Ok(new MeasureManagementResponse(response, operationOk));

            } catch (Exception e) {

                Debug.WriteLine(e.ToString());

                return RequestResponse.NotOk(new MeasureManagementResponse(null, false));
            }
        }

        private async Task<IRequestResponse<MeasureManagementResponse>> DeleteMeasure(MeasureManagementRequest request) {
            try {
                var response = await manager.DeleteMeasure(request.Measure.Id);

                return RequestResponse.Ok(new MeasureManagementResponse(null, response));

            } catch (Exception e) {

                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new MeasureManagementResponse(null, false));
            }
        }
    }
}
