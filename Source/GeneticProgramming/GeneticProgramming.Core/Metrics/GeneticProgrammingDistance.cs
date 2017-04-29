using System;
using System.Collections.Generic;
using GeneticProgramming.Core.Programs;
using GeneticProgramming.Core.Programs.Terminals;
using GeneticProgramming.Data.Contracts;
using Metadata.Attributes;
using Metadata.Distance.HungarianAlgorithm;

namespace GeneticProgramming.Core.Metrics
{
    public abstract class GeneticProgrammingDistance<T> : IAttributeMetric where T : AttributeMetadata
    {
        public IGpDummyDistance<T> DummyDistance { get; set; }
        public bool RepairToSemimetric { get; set; }
        protected IProgramNode Program;
        public CurrrentValueTuple<T> CurrentValues { get; set; }

        public abstract Dictionary<string, Func<string, double>> GetTerminalDictionary();

        protected GeneticProgrammingDistance(Program program, IGpDummyDistance<T> dummyDistance, bool repairToSemimetric)
        {
            DummyDistance = dummyDistance;
            RepairToSemimetric = repairToSemimetric;
            CurrentValues = new CurrrentValueTuple<T>();
            Program = ProgramConverter.Convert(program, GetTerminalDictionary());
        }

        public double GetNotDummyDistanceWithoutRepairment(T attributeA, T attributeB)
        {
            CurrentValues.CurrentValueLeft = attributeA;
            CurrentValues.CurrentValueRight = attributeB;
            var toReturn = Program.CurrentValue;
            if (toReturn == int.MinValue)
            {
                return int.MaxValue;
            }
            return toReturn;
        }

        public double GetNotDummyDistance(T attributeA, T attributeB)
        {
            var toReturn = GetNotDummyDistanceWithoutRepairment(attributeA, attributeB);
            if (RepairToSemimetric)
            {
                toReturn = Math.Abs(toReturn/2);
                var second = Math.Abs(GetNotDummyDistanceWithoutRepairment(attributeB, attributeA)/2);
                toReturn += second;
            }
            return toReturn;
        }

        public double GetDistance(AttributeMetadata attributeA, AttributeMetadata attributeB)
        {
            if (attributeA is DummyAttribute || attributeB is DummyAttribute)
            {
                return DummyDistance.GetDistance(attributeA, attributeB, GetNotDummyDistance);
            }
            if (!(attributeA is T) || !(attributeB is T))
            {
                throw new Exception();
            }
            var toReturn = GetNotDummyDistance((T) attributeA, (T) attributeB);
            return toReturn;
        }
    }
}
