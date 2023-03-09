using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplatesProfile : AutoMapper.Profile {
        public TemplatesProfile() {
            CreateMap<Template, TemplateListResponse.ListItem>();
        }
    }
}
