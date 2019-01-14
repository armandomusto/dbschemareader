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
        
        #region Parameters
        public List<DbProvider> DbProviders { get; set; }
        public DbProvider SelectedDbProvider { get { return _selectedProvider; } set { Set<DbProvider>(ref _selectedProvider, value, "SelectedDbProvider"); } }
        public string Host { get { return _host; } set { Set<string>(ref _host, value, "Host"); } }
        public string Database { get { return _database; } set { Set<string>(ref _database, value, "Database"); } }
        public string User { get { return _user; } set { Set<string>(ref _user, value, "User"); } }
        public string Password { get { return _password; } set { Set<string>(ref _password, value, "Password"); } }
        public bool TrustedConnection { get { return _trustedConnection; } set { Set<bool>(ref _trustedConnection, value, "TrustedConnection"); } }
        public string ConnectionString { get { return _connectionString; } set { Set<string>(ref _connectionString, value, "ConnectionString"); } }
        public ObservableCollection<NodeItem> Schemas { get; set; }
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
            DbProviders.Add(new DbProvider() { Name = "PostgreSql", Type = DbProvider.POSTGRESQL, Provider = "Npqsql" });
            Schemas = new ObservableCollection<NodeItem>();
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

        private Task<bool> GetSchema()
        {
            var rdr = new DatabaseReader(ConnectionString, SelectedDbProvider.Provider);
            rdr.Owner = "dbo";
            _databaseSchema = rdr.ReadAll();
            Schemas = new ObservableCollection<NodeItem>(SchemaToTree.GenerateTreeList(_databaseSchema));
            RaisePropertyChanged(() => Schemas);
            return new Task<bool>(() => true);
        }

        //private bool IsConnectionStringValid()
        //{
        //    string connectionString = ConnectionString.Text.Trim();
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        errorProvider1.SetError(ConnectionString, "Should not be empty");
        //        return false;
        //    }
        //    try
        //    {
        //        var factory = DbProviderFactories.GetFactory(DataProviders.SelectedValue.ToString());
        //        var csb = factory.CreateConnectionStringBuilder();
        //        csb.ConnectionString = connectionString;
        //    }
        //    catch (NotSupportedException)
        //    {
        //        errorProvider1.SetError(ConnectionString, "Invalid connection string");
        //        return false;
        //    }
        //    catch (ArgumentException)
        //    {
        //        errorProvider1.SetError(ConnectionString, "Invalid connection string");
        //        return false;
        //    }
        //    catch (ConfigurationErrorsException)
        //    {
        //        errorProvider1.SetError(DataProviders, "This provider isn't available");
        //        return false;
        //    }

        //    errorProvider1.SetError(DataProviders, string.Empty);
        //    errorProvider1.SetError(ConnectionString, string.Empty);
        //    return true;
        //}
    }
}