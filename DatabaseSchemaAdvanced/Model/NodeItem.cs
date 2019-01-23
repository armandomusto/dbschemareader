using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class NodeItem : INotifyPropertyChanged
    {
        bool _isSelected;
        bool _isExpanded;

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
        [Ignore]
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected")); } }
        [Ignore]
        public bool IsExpanded { get { return _isExpanded; } set { _isExpanded = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsExpanded")); } }

        public NodeItem(NodeType type)
        {
            Type = type;
            this.Items = new List<NodeItem>();
            if (type == NodeType.Schema || type == NodeType.Tables)
                IsExpanded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
