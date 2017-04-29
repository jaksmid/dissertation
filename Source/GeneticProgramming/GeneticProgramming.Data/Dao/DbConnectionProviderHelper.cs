using System;
using System.Data.Common;
using System.Data.SQLite;

namespace GeneticProgramming.Data.Dao
{
    public class DbConnectionProviderHelper
    {

        public static DbConnection GetWinConnection(string connectionString)
        {
            var con = new SQLiteConnection(connectionString);
            Console.WriteLine("Connected to sqlLite using win libs: " + con);
            return con;
        }
    }
}
