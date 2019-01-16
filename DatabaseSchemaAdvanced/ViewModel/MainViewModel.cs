using DatabaseSchemaAdvanced.Helper;
using DatabaseSchemaAdvanced.Model;
using DatabaseSchemaAdvanced.Service;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;

namespace DatabaseSchemaAdvanced.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        IDataService _dataService;
        DbProvider _selectedProvider;
        DatabaseSchema _databaseSchema;
        string _host;
        string _database;
        string _user;
        string _password;
        bool _trustedConnection;
        string _connectionString;
        string _schemaOwner;
        NodeItem _selectedNode;
        NodeItem _selectedTable;

        #region Parameters
        public List<DbProvider> DbProviders { get; set; }
        public DbProvider SelectedDbProvider { get { return _selectedProvider; } set { Set<DbProvider>(ref _selectedProvider, value, "SelectedDbProvider"); } }
        public string Host { get { return _host; } set { Set<string>(ref _host, value, "Host"); } }
        public string Database { get { return _database; } set { Set<string>(ref _database, value, "Database"); } }
        public string User { get { return _user; } set { Set<string>(ref _user, value, "User"); } }
        public string Password { get { return _password; } set { Set<string>(ref _password, value, "Password"); } }
        public string SchemaOwner { get { return _schemaOwner; } set { Set<string>(ref _schemaOwner, value, "SchemaOwner"); } }
        public bool TrustedConnection { get { return _trustedConnection; } set { Set<bool>(ref _trustedConnection, value, "TrustedConnection"); } }
        public string ConnectionString { get { return _connectionString; } set { Set<string>(ref _connectionString, value, "ConnectionString"); } }
        public ObservableCollection<NodeItem> Schemas { get; set; }
        public NodeItem SelectedNodeItem { get { return _selectedNode; } set { Set<NodeItem>(ref _selectedNode, value, "SelectedNodeItem"); ShowTable(); } }
        public NodeItem SelectedTable { get { return _selectedTable; } set { Set<NodeItem>(ref _selectedTable, value, "SelectedTable");} }

        #endregion

        #region Commands
        public RelayCommand ReadSchemaCommand { get; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            //Initialize databases
            DbProviders = new List<DbProvider>();
            DbProviders.Add(new DbProvider() { Name = "Sql Server", Type = DbProvider.SQLSERVER, Provider = "System.Data.SqlClient" });
            DbProviders.Add(new DbProvider() { Name = "PostgreSql", Type = DbProvider.POSTGRESQL, Provider = "Npgsql" });
            Schemas = new ObservableCollection<NodeItem>();
            SelectedDbProvider = DbProviders.FirstOrDefault(a => a.Type == Properties.Settings.Default.DbProvider);
            Host = Properties.Settings.Default.HostName;
            Database = Properties.Settings.Default.Database;
            User = Properties.Settings.Default.UserName;
            Password = Properties.Settings.Default.Password;
            SchemaOwner = Properties.Settings.Default.SchemaOwner;
            TrustedConnection = Properties.Settings.Default.TrustedConnection;
            ReadSchemaCommand = new RelayCommand(ReadSchema);
        }

        private void ReadSchema()
        {
            
            //Create connection string
            if(SelectedDbProvider != null)
            {
                if(SelectedDbProvider.Type == DbProvider.SQLSERVER)
                {
                    if(TrustedConnection)
                    {
                        ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Trusted_Connection=True;", Host, Database);
                    }
                    else
                    {
                        ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", Host, Database,User,Password);
                    }
                }
                if (SelectedDbProvider.Type == DbProvider.POSTGRESQL)
                {
                    ConnectionString = string.Format("Host={0};Database={1};User Id={2};Password={3}; Minimum Pool Size=0; Command Timeout=0;", Host, Database, User, Password);
                }
                if (!IsConnectionStringValid())
                    return;
                Task tsk = new Task(async () => 
                {
                    await GetSchema();
                });
                tsk.Start();
            }
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private Task GetSchema()
        {
            var rdr = new DatabaseReader(ConnectionString, SelectedDbProvider.Provider);
            rdr.Owner = "public";
            _databaseSchema = rdr.ReadAll();
            Schemas = new ObservableCollection<NodeItem>(SchemaToTree.GenerateTreeList(_databaseSchema));
            RaisePropertyChanged(() => Schemas);
            return new Task<bool>(() => true);
        }

        private void ShowTable()
        {
            if(_selectedNode != null && _selectedNode.Type == NodeType.Table)
            {
                SelectedTable = _selectedNode;
            }
        }
        private bool IsConnectionStringValid()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                //errorProvider1.SetError(ConnectionString, "Should not be empty");
                return false;
            }
            try
            {
                var factory = DbProviderFactories.GetFactory(SelectedDbProvider.Provider);
                var csb = factory.CreateConnectionStringBuilder();
                csb.ConnectionString = ConnectionString;
            }
            catch (NotSupportedException)
            {
                //errorProvider1.SetError(ConnectionString, "Invalid connection string");
                return false;
            }
            catch (ArgumentException)
            {
                //errorProvider1.SetError(ConnectionString, "Invalid connection string");
                return false;
            }
            catch (ConfigurationErrorsException)
            {
                //errorProvider1.SetError(DataProviders, "This provider isn't available");
                return false;
            }
            Properties.Settings.Default.DbProvider = SelectedDbProvider.Type;
            Properties.Settings.Default.HostName = Host;
            Properties.Settings.Default.Database = Database;
            Properties.Settings.Default.UserName = User;
            Properties.Settings.Default.Password = Password;
            Properties.Settings.Default.SchemaOwner = SchemaOwner;
            Properties.Settings.Default.TrustedConnection = TrustedConnection;
            Properties.Settings.Default.Save();
            //errorProvider1.SetError(DataProviders, string.Empty);
            //errorProvider1.SetError(ConnectionString, string.Empty);
            return true;
        }
    }
}