using DatabaseSchemaAdvanced.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseSchemaAdvanced
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            string filter = ((TextBox)e.Source).Text;
            if(treeviewSchema.Items.Count > 0 && filter != null && filter.Length > 2)
            {
                NodeItem root = treeviewSchema.Items[0] as NodeItem;
                if(root.Items[0] != null && root.Items[0].Items != null)
                {
                    foreach (var item in root.Items[0].Items)
                    {
                        if (((NodeItem)item).Type == NodeType.Table && ((NodeItem)item).Name.Contains(filter))
                        {
                            ((NodeItem)item).IsSelected = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
