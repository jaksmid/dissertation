using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadata.Global;

namespace Metadata.Distance
{
    public class DistanceMatrix
    {
        private readonly MetadataCollection _metadatas;
        private readonly Dictionary<String,int> _nameToIdDictionary=new Dictionary<string, int>();
        private readonly DistanceEntry[,] _distanceMatrix;
        public List<string> MetadataNames { get; private set; }

        public DistanceEntry[,] DistanceEntries { get { return _distanceMatrix; } }

        public IEnumerable<DistanceEntry> SortByDistance(string originDatasetName, bool ascending=true)
        {
            int rowId = _nameToIdDictionary[originDatasetName];
            var relevantEntries = GetRowEntries(rowId);
            if (ascending)
            {
                return relevantEntries.OrderBy(e => e.Distance);
            }
            return relevantEntries.OrderByDescending(e => e.Distance);
        }

        private IEnumerable<DistanceEntry> GetRowEntries(int rowId)
        {
            var results = new List<DistanceEntry>();
            for (int i = 0; i < _metadatas.Metadatas.Count(); i++)
            {
                if (i == rowId)
                {
                    continue;
                }
                results.Add(_distanceMatrix[rowId,i]);
            }
            return results;
        }

        public DistanceMatrix(MetadataCollection metadatas, IMetadataMetric metric, bool autoSymmetry=true)
        {
            _metadatas = metadatas;
            MetadataNames = metadatas.Metadatas.Select(x => x.Name).ToList();
            var datasets = _metadatas.Metadatas.ToList();
            var nrOfDatasets = datasets.Count();
            _distanceMatrix = new DistanceEntry[nrOfDatasets, nrOfDatasets];
            var datasetIndexes = new List<int>();
            for (int i = 0; i < nrOfDatasets; i++)
            {
                _nameToIdDictionary.Add(datasets[i].Name, i);
                datasetIndexes.Add(i);
            }
            for (int i = 0; i < nrOfDatasets; i++)
            {
                ComputeDistanceForDatasetWithIndex(i, datasets, nrOfDatasets, metric, autoSymmetry);
            }
        }

        public void ComputeDistanceForDatasetWithIndex(int i, List<DatasetMetadata> datasets, int nrOfDatasets, IMetadataMetric metric, bool autoSymmetry)
        {
            DatasetMetadata datasetA = datasets[i];
            for (int j = i; j < nrOfDatasets; j++)
            {
                if (j == i)
                {
                    _distanceMatrix[i, j] = new DistanceEntry(datasetA, datasetA, 0);
                }
                else
                {
                    DatasetMetadata datasetB = datasets[j];
                    double distance = metric.MeasureDistance(datasetA, datasetB);
                    _distanceMatrix[i, j] = new DistanceEntry(datasetA, datasetB, distance);
                    if (autoSymmetry)
                    {
                        _distanceMatrix[j, i] = new DistanceEntry(datasetB, datasetA, distance);
                    }
                    else
                    {
                        distance = metric.MeasureDistance(datasetB, datasetA);
                        _distanceMatrix[j, i] = new DistanceEntry(datasetB, datasetA, distance);
                    }
                }
            }   
        }

        public string PrintDistanceMatrix()
        {
            var sb=new StringBuilder("[");
            int nrOfDatasets = _metadatas.Metadatas.Count();
            sb.Append("['SourceTarget'");
            for (int i = 0; i < nrOfDatasets; i++)
            {
                //print header
                sb.Append(",'" + _distanceMatrix[0,i].TargetName+"'");
            }
            sb.Append("]");
            for (int i = 0; i < nrOfDatasets; i++)
            {
                sb.Append(",[");
                string datasetName = _distanceMatrix[i, 0].SourceName;
                sb.Append("'"+datasetName+ "'");
                for (int j = 0; j < nrOfDatasets; j++)
                {
                    sb.Append(","+_distanceMatrix[i, j].Distance);
                }
                sb.Append("]");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
