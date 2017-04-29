using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Core.Metrics;
using GeneticProgramming.Data.Contracts;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Global;

namespace GeneticProgramming.Core.Fitness
{
    public class AttributeMetricSimilarity
    {

        public GeneticProgramNumericalDistance NumericalDistance { get; set; }
        public GeneticProgramCategoricalDistance CategoricalDistance { get; set; }

        public AttributeMetricSimilarity(Program numericalProgram, Program categoricalProgram, bool repairToSemimetric, GpDummyDistancePair dummyDistancePair)
         {
            NumericalDistance = new GeneticProgramNumericalDistance(numericalProgram, dummyDistancePair.NumericalDummyDistance, repairToSemimetric);
            CategoricalDistance = new GeneticProgramCategoricalDistance(categoricalProgram, dummyDistancePair.CategoricalDummyDistance, repairToSemimetric);
         }

        public double ComputeAttributeMetricSimilarity(List<DatasetMetadata> metadatas)
        {
            int total = 0;
            int triangleErrors = 0;
            var categorical = metadatas.SelectMany(x => x.CategoricalAttributes).ToList();
            for (int i = 0; i < categorical.Count; i++)
            {
                var x = categorical[i];
                for (int j = 0; j < categorical.Count; j++)
                {
                    var y = categorical[j];
                    var xyDistance = CategoricalDistance.GetDistance(x, y);
                    for (int z = 0; z < categorical.Count; z++)
                    {
                        total++;
                        var middle = categorical[z];
                        var xz = CategoricalDistance.GetDistance(x, middle);
                        var zy = CategoricalDistance.GetDistance(middle, y);
                        if (xyDistance > xz + zy)
                        {
                            triangleErrors++;
                        }
                    }
                }
            }
            var numerical = metadatas.SelectMany(x => x.NumericalAttributes).ToList();
            for (int i = 0; i < numerical.Count; i++)
            {
                var x = numerical[i];
                for (int j = 0; j < numerical.Count; j++)
                {
                    var y = numerical[j];
                    var xyDistance = NumericalDistance.GetDistance(x, y);
                    for (int z = 0; z < numerical.Count; z++)
                    {
                        total++;
                        var middle = numerical[z];
                        var xz = NumericalDistance.GetDistance(x, middle);
                        var zy = NumericalDistance.GetDistance(middle, y);
                        if (xyDistance > xz + zy)
                        {
                            triangleErrors++;
                        }
                    }
                }
            }
            return 1-((double) triangleErrors/total);
        }
    }
}