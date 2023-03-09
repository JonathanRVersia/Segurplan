
using System.Collections.Generic;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.BusinessObjects {
    public class PlanBusinessAddress {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Delegation> Delegations { get; internal set; }
    }
}
