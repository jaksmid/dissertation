using System;
using System.Configuration;

namespace GeneticProgramming.Data.Dao
{
    public class DbEntitiesProviderFactory
    {
        public IDbEntitiesProvider CreateDbEntitiesProvider(bool useSqlLite=true, bool readOnly = true)
        {
            bool hasDirectConnectionToDb = true;
            string hasDbAccess = ConfigurationManager.AppSettings["AccessToDb"];
            if (hasDbAccess == "false")
            {
                hasDirectConnectionToDb = false;
                Console.WriteLine("Created without direct access to DB");
            }
            else
            {
                Console.WriteLine("Created with direct access to DB");
            }
            if (hasDirectConnectionToDb)
            {
                return new DbEntitiesProvider(readOnly,useSqlLite);
            }
            return new DbEntitiesProvider(readOnly);
        }
    }
}
