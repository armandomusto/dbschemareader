using DatabaseSchemaAdvanced.Model;
using DatabaseSchemaReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSchemaAdvanced.Service
{
    public interface IDataService
    {
        void SetColumnsDescriptionProperties(string connectionString, List<NodeItem> columns, string providerType);
        void ExportSchemaToCsv(List<NodeItem> columns, string filePath);
        List<NodeItem> ImportSchemaFromCsv(string filePath);
    }
}
