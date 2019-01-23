using DatabaseSchemaAdvanced.Model;
using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSchemaAdvanced.Helper
{
    static class SchemaToTree
    {
        private const string RightClickToScript = "Right click to script";

        public static List<NodeItem> GenerateTreeList(IList<DatabaseTable> tables)
        {
            List<NodeItem> list = new List<NodeItem>();
            NodeItem treeRoot = new NodeItem(NodeType.Schema) { Name = "Schema" };
            list.Add(treeRoot);
            var tableRoot = new NodeItem(NodeType.Tables) { Schema = treeRoot.Name, Name = "Tables" };
            treeRoot.Items.Add(tableRoot);

            foreach (var table in tables)
            {
                FillTables(tableRoot, table);
            }
            
            return list;

        }

        public static List<NodeItem> GenerateTreeList(DatabaseSchema schema)
        {
            List<NodeItem> list = new List<NodeItem>();
            foreach( var schemaObj in schema.Schemas)
            {
                NodeItem treeRoot = new NodeItem(NodeType.Schema) { Name = schemaObj.Name };
                list.Add(treeRoot);
                FillTables(treeRoot, schema);
            }
            if(schema.Schemas.Count == 0)
            {
                NodeItem treeRoot = new NodeItem(NodeType.Schema) {Name = "Schema" };
                list.Add(treeRoot);
                FillTables(treeRoot, schema);
            }
            return list;

        }

        private static void FillTables(NodeItem treeRoot, DatabaseTable table)
        {
            NodeItem tableNode = new NodeItem(NodeType.Table);
            foreach (var table1 in table.Columns.OrderBy(x => x.SchemaOwner).ThenBy(x => x.Name))
            {
                var name = table.Name;
                if (!string.IsNullOrEmpty(table.SchemaOwner)) name = table.SchemaOwner + "." + name;
                tableNode.Name = name;
                tableNode.Schema = treeRoot.Name ;
                foreach (var column in table.Columns)
                {
                    FillColumn(tableNode, column);
                }
            }
            treeRoot.Items.Add(tableNode);
        }

        private static void FillTables(NodeItem treeRoot, DatabaseSchema schema)
        {
            var tableRoot = new NodeItem(NodeType.Tables) {Schema=treeRoot.Name, Name = "Tables" };
            //tableRoot.Tag = schema;
            //tableRoot.ToolTipText = RightClickToScript;
            treeRoot.Items.Add(tableRoot);

            foreach (var table in schema.Tables.OrderBy(x => x.SchemaOwner).ThenBy(x => x.Name))
            {
                var name = table.Name;
                if (!string.IsNullOrEmpty(table.SchemaOwner)) name = table.SchemaOwner + "." + name;
                var tableNode = new NodeItem(NodeType.Table) { Name = name, Schema = treeRoot.Name };
                //tableNode.Tag = table;
                //tableNode.ToolTipText = RightClickToScript;
                tableRoot.Items.Add(tableNode);
                foreach (var column in table.Columns)
                {
                    FillColumn(tableNode, column);
                }
                //FillConstraints(table, tableNode);
                //FillTriggers(table, tableNode);
                //FillIndexes(table, tableNode);
            }
        }

        private static void FillColumn(NodeItem tableNode, DatabaseColumn column)
        {
            var sb = new StringBuilder();
            sb.Append(column.Name);
            var colNode = new NodeItem(NodeType.Column) { Name = sb.ToString(), Description = column.Description,
                Schema = tableNode.Schema, Table = tableNode.Name };
            tableNode.Items.Add(colNode);
        }
    }
}
