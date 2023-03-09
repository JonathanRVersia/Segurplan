//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Segurplan.Core.BusinessManagers;
//using Segurplan.DataAccessLayer.DataAccessManagers;
//using Segurplan.FrameworkExtensions.MediatR;

//namespace Segurplan.Core.Actions.Administration.Activitys {
//    public class ActivityListRequestHandler : IRequestHandler<ActivityListRequest, IRequestResponse<ActivityListResponse>> {


//        protected readonly ActivityDam activityDam;

//        public ActivityListRequestHandler(ActivityDam activityDam) {
//            this.activityDam = activityDam;
//        }


//        public async Task<IRequestResponse<ActivityListResponse>> Handle(ActivityListRequest request, CancellationToken cancellationToken) {


//            return await GetPreventiveMeasureList(request);

//        }



//        private async Task<IRequestResponse<ActivityListResponse>> GetPreventiveMeasureList(ActivityListRequest request) {

//            try {
//                var manager = new ActivityListManager(activityDam);
//                var activityList = await manager.GetActivityList(request.TableState, request.TableFilter);

//                return RequestResponse.Ok(new ActivityListResponse(activityList, manager.FilteredActivitys));

//            } catch (Exception e) {
//                Debug.WriteLine(e.ToString());
//                return RequestResponse.NotOk(new ActivityListResponse(null, -1));
//            }

//        }



//    }
//}
