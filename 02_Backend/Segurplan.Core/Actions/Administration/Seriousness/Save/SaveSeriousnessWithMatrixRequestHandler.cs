using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Segurplan.Core.Actions.Administration.Seriousness.Save {
    public class SaveSeriousnessWithMatrixRequestHandler : IRequestHandler<SaveSeriousnessWithMatrixRequest, IRequestResponse<SaveSeriousnessWithMatrixResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveSeriousnessWithMatrixRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<SaveSeriousnessWithMatrixResponse>> Handle(SaveSeriousnessWithMatrixRequest request, CancellationToken cancellationToken) {

            //var seriousnessName = request.TableMatrixValues.Find(x => !string.IsNullOrEmpty(x.SeriousnessName)).SeriousnessName;
            bool correctSave = false;

            if (request.Seriousness.Id != default) {
                correctSave = await EditBehaviourAsync(request) > 0;
            } else {
                correctSave = await CreateBehaviourAsync(request) > 0;
            }

            if (!correctSave)
                return RequestResponse.Error<SaveSeriousnessWithMatrixResponse>();

            return RequestResponse.Ok(new SaveSeriousnessWithMatrixResponse());
        }

        private async Task<int> CreateBehaviourAsync(SaveSeriousnessWithMatrixRequest request) {
            var seriousness = mapper.Map<DataAccessLayer.Database.DataTransferObjects.Seriousness>(request.Seriousness);

            context.Seriousness.Add(seriousness);
            return await context.SaveChangesAsync();
        }

        private async Task<int> EditBehaviourAsync(SaveSeriousnessWithMatrixRequest request) {
            //context.RiskLevelBySeriousnessAndProbabilities.Remove(new RiskLevelBySeriousnessAndProbability { SeriousnessId = request.Seriousness.Id });
            var seriousness = context.Seriousness.Where(s => s.Id == request.Seriousness.Id).Include(s => s.RiskLevelBySeriousnessAndProbabilities).FirstOrDefault();

            if (seriousness is null)
                return default;

            seriousness = mapper.Map<DataAccessLayer.Database.DataTransferObjects.Seriousness>(request.Seriousness);

            context.Seriousness.Update(seriousness);
            return await context.SaveChangesAsync();
        }
    }
}
