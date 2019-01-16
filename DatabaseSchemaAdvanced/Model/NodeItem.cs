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
        public string Schema { get; set; }
        public string Table { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// TABLE
        /// COLUMN
        /// </summary>
        public NodeType Type { get; private set; }
        public bool IsNew { get; set; }
        public ObservableCollection<NodeItem> Items { get; set; }

        public NodeItem(NodeType type)
        {
            Type = type;
            this.Items = new ObservableCollection<NodeItem>();
        }
    }
}
