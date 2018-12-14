using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.Common
{
    public class ProcModel
    {
        public string Name { get; set; }
        public List<ParameterModel> Parameters { get; set; }
        public bool ReturnTable { get; set; }
    }

    public class ParameterModel
    {
        public string Name { get; set; }
        
        public bool Required { get; set; }
        public string DbType { get; set; }
        public string DataType { get; set; }
        public int? Size { get; set; }
        public int? Precision { get; set; }
        public bool IsNullable { get; set; }
        public bool IsOutput { get; set; }
    }


}
