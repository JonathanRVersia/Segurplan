using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Family.Save {
    public class SaveFamilyRequestHandler : IRequestHandler<SaveFamilyRequest, IRequestResponse<SaveFamilyResponse>> {

        private readonly SegurplanContext context;
        private readonly IMapper mapper;

        public SaveFamilyRequestHandler(SegurplanContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<SaveFamilyResponse>> Handle(SaveFamilyRequest request, CancellationToken cancellationToken) {

            bool correctSave = false;

            if (request.family.Id != default) {
                correctSave = await EditFamilyAsync(request) > 0;
            } else {
                correctSave = await CreateFamilyAsync(request) > 0;
            }

            if (!correctSave)
                return RequestResponse.Error<SaveFamilyResponse>();

            return RequestResponse.Ok(new SaveFamilyResponse());
        }

        private async Task<int> EditFamilyAsync(SaveFamilyRequest request) {
            var family = await context.ArticleFamily.FirstOrDefaultAsync(r => r.Id == request.family.Id);

            if (family is null) return default;

            family = mapper.Map<DataAccessLayer.Database.DataTransferObjects.ArticleFamily>(request.family);
            family.ModifiedBy = request.UserId;
            family.UpdateDate = DateTime.Now;

            context.ArticleFamily.Update(family);
            return await context.SaveChangesAsync();
        }

        private async Task<int> CreateFamilyAsync(SaveFamilyRequest request) {
            var family = mapper.Map<DataAccessLayer.Database.DataTransferObjects.ArticleFamily>(request.family);
            family.CreatedBy = request.UserId;
            family.ModifiedBy = request.UserId;
            context.ArticleFamily.Add(family);
            return await context.SaveChangesAsync();
        }

        
    }
}
