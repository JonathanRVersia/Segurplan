using System;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Job {
        public Int64 Id { get; set; }

        public Int64 StateId { get; set; }

        public string StateName { get; set; }

        public string InvocationData { get; set; }

        public string Arguments { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpireAt { get; set; }

    }
}
