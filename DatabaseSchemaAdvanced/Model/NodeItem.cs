using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSchemaAdvanced.Model
{
    public enum NodeType
    {
        Schema,
        Tables,
        Table,
        Column
    }
    public class NodeItem
    {
        [Ignore]
        public string Schema { get; set; }
        [Name("Table"), Index(1)]
        public string Table { get; set; }
        [Name("Column"), Index(2)]
        public string Name { get; set; }
        [Name("Description"), Index(3)]
        public string Description { get; set; }
        [Ignore]
        /// <summary>
        /// TABLE
        /// COLUMN
        /// </summary>
        public NodeType Type { get; private set; }
        [Ignore]
        public bool IsNew { get; set; }
        [Ignore]
        public List<NodeItem> Items { get; set; }

        public NodeItem(NodeType type)
        {
            Type = type;
            this.Items = new List<NodeItem>();
        }
    }
}
