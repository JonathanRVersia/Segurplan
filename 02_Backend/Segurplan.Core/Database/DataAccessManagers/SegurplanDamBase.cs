using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Database.DataTransferObjects;

namespace Segurplan.Core.Database.DataAccessManagers {
    public abstract class SegurplanDamBase<Entity> {
        public readonly SegurplanContext context;

        public SegurplanDamBase(SegurplanContext dbContext) {
            context = dbContext;
        }

        public abstract Entity SelectById(int id);

        public abstract int SelectCount();
    }
}
