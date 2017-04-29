using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticProgramming.Data.Dao;
using Metadata.Mining;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class MetadataCopyTask : IComputationTask
    {
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        private bool _saveDataQualities;

        public MetadataCopyTask(bool saveMissingQuantities = false, bool saveDataQualities = false)
        {
            _saveMissingQuantities = saveMissingQuantities;
            _saveDataQualities = saveDataQualities;
        }

        private bool _saveMissingQuantities { get; set; }

        public void Execute()
        {
            using (var dbProvider =DbEntitiesProviderFactory.CreateDbEntitiesProvider())
            {
                var miner = new OpenMlMiner();
                if(_saveMissingQuantities) miner.AddMissingDataQualities();
                if (_saveDataQualities) miner.SaveDataQualities();
                miner.UpdateDeletedInOpenMl();

            }
        }
    }
}