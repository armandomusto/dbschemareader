using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSchemaAdvanced.Model
{
    public class NodeItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public ObservableCollection<NodeItem> Items { get; set; }

        public NodeItem()
        {
            this.Items = new ObservableCollection<NodeItem>();
        }
    }
}
