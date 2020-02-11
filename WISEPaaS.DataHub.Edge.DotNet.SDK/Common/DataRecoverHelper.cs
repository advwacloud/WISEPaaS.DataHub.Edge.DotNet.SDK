using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WISEPaaS.SCADA.DotNet.SDK.Model;

namespace WISEPaaS.SCADA.DotNet.SDK
{
    public class DataRecoverHelper
    {
        private const string dbFileName = "recover.sqlite";

        private const int DefaultReadRecordCount = 10;
        private const int DeafultWriteRecordCount = 1000;

        private string _connString;

        private string _dbFilePath;
        private object _lockObj;
        private SQLiteConnection _conn;

        public DataRecoverHelper()
        {
            _lockObj = new object();

            _dbFilePath = Path.Combine( Directory.GetCurrentDirectory(), dbFileName );
            _connString = "data source=" + _dbFilePath;
            _conn = new SQLiteConnection( _connString );

            if ( !File.Exists( _dbFilePath ) )
                _conn.Execute( @"CREATE TABLE Data (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, message TEXT NOT NULL)" );   // create table "Data"

            _conn.Open();
            _conn.Execute( @"VACUUM" ); // release storage
        }

        ~DataRecoverHelper()
        {
            try
            {
                _conn.Close();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
            }
        }

        public bool DataAvailable()
        {
            bool result = false;
            try
            {
                lock ( _lockObj )
                {
                    var list = _conn.Query<DataModel>( "SELECT * FROM Data LIMIT 1" );
                    if ( list != null && list.Count() > 0 )
                        result = true;
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
            }
            return result;
        }

        public List<string> Read( int count = DefaultReadRecordCount )
        {
            List<string> messages = new List<string>();

            try
            {
                var models = _conn.Query<DataModel>( "SELECT * FROM Data LIMIT @Count", new { Count = count } );
                var idList = models.Select( x => x.Id ).ToArray();
                if ( idList.Count() > 0 )
                {
                    _conn.Execute( "DELETE FROM Data WHERE id IN @Ids", new { Ids = idList } );
                    messages = models.Select( x => Compression.DecompressFromBase64String( x.Message ) ).ToList();
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex );
            }

            return messages;
        }

        public bool Write( List<string> messages )
        {
            using ( var trans = _conn.BeginTransaction() )
            {
                try
                {
                    foreach ( string message in messages )
                    {
                        if ( string.IsNullOrEmpty( message ) )
                            continue;

                        _conn.Execute( "INSERT INTO Data (message) VALUES (@Message)", new { Message = Compression.CompressToBase64String( message ) }, trans );
                    }
                    trans.Commit();
                    return true;
                }
                catch ( Exception ex )
                {
                    Console.WriteLine( ex );
                    trans.Rollback();
                }
            }
            return false;
        }
    }
}
