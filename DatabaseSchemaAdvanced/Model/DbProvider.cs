using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSchemaAdvanced.Model
{
    public class DbProvider
    {
        public const string SQLSERVER = "0";
        public const string POSTGRESQL = "1";

        public string Name { get; set; }
        public string Type { get; set; }
        public string Provider { get; set; }
    }
}
