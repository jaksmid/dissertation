using GeneticProgramming.Data.Dao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Metadata.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //TODO: move to data test
            DbEntitiesProvider prov=new DbEntitiesProvider(false);
            var res=prov.MetadataWithResults;
            string a = "";

        }
    }
}
