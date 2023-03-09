using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.BusinessObjects {
    public class ApplicationRisk {
        public ApplicationRisk () {

        }
        public ApplicationRisk(int id,int code,string name) {
            Id = id;
            Code = code;
            Name = name;
        }
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
