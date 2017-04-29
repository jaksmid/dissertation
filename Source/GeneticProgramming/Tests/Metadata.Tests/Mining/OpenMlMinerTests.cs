using System;
using Metadata.Mining;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Metadata.Tests.Mining
{
    [TestClass]
    public class OpenMlMinerTests
    {
        [TestMethod]
        public void TestListDatasetsOk()
        {
            var miner=new OpenMlMiner();
            var res = miner.GetAvailableDatasets();
            var a = "";
        }
    }
}
