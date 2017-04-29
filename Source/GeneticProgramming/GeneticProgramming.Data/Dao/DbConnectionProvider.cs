using System;
using System.Configuration;
using System.Data.Common;
//using System.Data.SQLite;
using System.IO;
using MySql.Data.MySqlClient;
using Mono.Data.Sqlite;

namespace GeneticProgramming.Data.Dao
{
    public class DbConnectionProvider
    {
        private readonly bool _useSqLite;
        private readonly bool _readOnly;

        public DbConnectionProvider(bool useSqLite, bool readOnly)
        {
            _useSqLite = useSqLite;
            _readOnly = readOnly;
        }

        public DbConnection GetConnection()
        {
            if (_useSqLite)
            {
                string connectionString = "Data Source=SqlLite" + Path.DirectorySeparatorChar +
                                          "pikater.db;Version=3;";
                if (_readOnly)
                {
                    connectionString += "Read Only=True;";
                }
                //Data Source = c:\mydb.db; Version = 3; Read Only = True;
                var platform = RunningPlatform();
                Console.WriteLine("Connecting to sqlLite: " + connectionString + "platform: "+platform);
                if (platform == Platform.Windows)
                {
                    return DbConnectionProviderHelper.GetWinConnection(connectionString);
                }
                var con = new SqliteConnection(connectionString);
                Console.WriteLine("Connected to sqlLite using linux libs: " + con);
                return con;
            }
            var connString = ConfigurationManager.ConnectionStrings["pikaterDb"].ConnectionString;
            Console.WriteLine("Connecting to MySql: " + connString);
            var connection = new MySqlConnection(connString);
            return connection;
        }

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    // Well, there are chances MacOSX is reported as Unix instead of MacOSX.
                    // Instead of platform check, we'll do a feature checks (Mac specific root folders)
                    if (Directory.Exists("/Applications")
                        & Directory.Exists("/System")
                        & Directory.Exists("/Users")
                        & Directory.Exists("/Volumes"))
                        return Platform.Mac;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.Mac;

                default:
                    return Platform.Windows;
            }
        }
    }
}
